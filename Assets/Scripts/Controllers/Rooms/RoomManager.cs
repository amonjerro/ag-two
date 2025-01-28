using SaveGame;
using System.Collections.Generic;
using UnityEngine;
using Tasks;

namespace Rooms
{
    public class RoomManager : MonoBehaviour
    {

        public List<Room> availableRooms;
        public List<RoomData> roomData;
        private Dictionary<RoomType, RoomData> roomDataDict;
        private Dictionary<(int, int), Room> roomMap;
        private Dictionary<(int, int), RoomTile> roomTiles;

        [SerializeField]
        int fortressWidth;

        [SerializeField]
        int fortressHeight;

        [SerializeField]
        ParticleSystem roomParticleSystem;

        private void Awake()
        {
            roomMap = new Dictionary<(int, int), Room>();
            roomDataDict = new Dictionary<RoomType, RoomData> ();
            roomTiles = new Dictionary<(int, int), RoomTile>();
            availableRooms = new List<Room> ();
            TimeManager.Tick += HandleTick;

            SetupRoomDataDict();
            SetupInitialRooms();
        }

        public void LoadTasks(Queue<Task> tasks)
        {
            Task activeTask;
            while (tasks.Count > 0) { 
                activeTask = tasks.Dequeue();
                switch (activeTask.TaskType) {
                    case TaskType.Quest:
                    case TaskType.Exploration:
                        Debug.Log(activeTask.ToString());
                        Room opsRoom = roomMap[(0, 0)];
                        opsRoom.EnqueueTask(activeTask);
                        break;
                    case TaskType.Build:
                        BuildTask bdTask = (BuildTask)activeTask;
                        Room debris = roomMap[bdTask.RoomCoordinates];
                        debris.EnqueueTask(activeTask);
                        break;
                    default:
                        break;
                }
                
            }
        }

        private void SetupRoomDataDict()
        {
            roomDataDict.Clear();
            foreach (RoomData r in roomData) {
                roomDataDict.Add(r.roomType, r);
            }
        }

        private void SetupInitialRooms()
        {
            AddAvailableRoom(RoomFactory.MakeRoom(roomDataDict[RoomType.BRK]));
            AddAvailableRoom(RoomFactory.MakeRoom(roomDataDict[RoomType.LIB]));

            for (int x = 0; x < fortressWidth; x++)
            {
                for (int y = 0; y < fortressHeight; y++) {
                    if (y == 0 && x == 0)
                    {
                        roomMap.Add((x, y), RoomFactory.MakeRoom(roomDataDict[RoomType.OPS]));
                        continue;
                    }

                    // Add Debris Room
                    Room debrisRoom = RoomFactory.MakeRoom(roomDataDict[RoomType.DBR]);
                    RoomComponent buildableComponent = debrisRoom.GetRoomComponent(ComponentType.BUILDABLE);
                    if (buildableComponent != null)
                    {
                        BuildableComponent bc = buildableComponent as BuildableComponent;
                        bc.AddHeightCost(y);
                    }
                    roomMap.Add((x, y), debrisRoom);
                } 
            }
        }


        private void HandleTick()
        {
            foreach(KeyValuePair<(int, int), Room> pair in roomMap)
            {
                pair.Value.RoomTick();
            }
        }

        public void AddAvailableRoom(Room room)
        {
            availableRooms.Add(room);
        }

        public void RemoveAvailableRoom(Room room) { 
            availableRooms.Remove(room);
        }

        public void BeginRoomBuild(int x, int y, RoomType roomType)
        {

            // Do all the player feedback stuff  //
            // Update the room sprite to be the build sprite
            roomTiles[(x, y)].SetSprite(roomDataDict[RoomType.BLD].roomSprite, Color.white);

            // Move the particle system to the right place
            (int, int) worldPosition = KeyToWorld((x, y));
            roomParticleSystem.transform.position = new Vector3(worldPosition.Item1, worldPosition.Item2, 0);
            roomParticleSystem.Play();

            // Do all the game logic stuff //
            Room room = roomMap[(x, y)];
            RoomComponent component = room.GetRoomComponent(ComponentType.BUILDABLE);
            
            // Manage the resources
            GameInstance.gold -= component.GetCost();

            // Create the task
            BuildTask buildTask = TaskFactory.MakeBuildTask((x, y), roomType);
            room.EnqueueTask(buildTask);

            Debug.Log("Build Task Created Succesfully");
            ServiceLocator.Instance.GetService<UIManager>().HideBuildInterface();
        }

        public Sprite GetSpriteByType(RoomType type) {
            return roomDataDict[type].roomSprite;
        }

        public bool IsRoomHovered(int worldX, int worldY)
        {

            return roomMap.ContainsKey(WorldToKey(worldX, worldY));
        }

        

        public AbsRoomClickEvent HandleClick(int worldX, int worldY)
        {
            Room room = roomMap[WorldToKey(worldX, worldY)];
            return room.HandleClick();

        }

        public (int, int) WorldToKey(int x, int y)
        {
            return ((x + 8) / 4, y / 2);
        }

        private (int, int) KeyToWorld((int, int) position) {
            return (4 * position.Item1 - 6, 2 * position.Item2 + 1);
        }

        public (int, int) WorldGridSnap(int x, int y)
        {
            (int, int) coordinates = WorldToKey(x, y);
            return KeyToWorld(coordinates);
        }

        public Room GetRoom(int x, int y) { 
            return roomMap[WorldToKey(x, y)];
        }

        public List<Room> FilterAvailableRooms(BuildRestrictions excludeRestriction)
        {
            List<Room> roomList = new List<Room>();

            foreach(Room room in availableRooms)
            {
                if (room.buildRestriction == excludeRestriction)
                {
                    continue;
                }
                roomList.Add(room);
            }

            return roomList;
        }

        public void RegisterTile((int, int) key, RoomTile tile)
        {
            roomTiles.Add(key, tile);
        }
    }
}