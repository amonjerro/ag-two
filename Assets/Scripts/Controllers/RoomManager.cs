using System.Collections.Generic;
using UnityEngine;

namespace Rooms
{
    public class RoomManager : MonoBehaviour
    {
        
        public List<Room> availableRooms;
        public List<RoomData> roomData;
        private Dictionary<RoomType, RoomData> roomDataDict;
        private Dictionary<(int, int), Room> roomMap;

        [SerializeField]
        int fortressWidth;

        [SerializeField]
        int fortressHeight;

        private void Awake()
        {
            roomMap = new Dictionary<(int, int), Room>();
            roomDataDict = new Dictionary<RoomType, RoomData> ();
            availableRooms = new List<Room> ();
            TimeManager.Tick += HandleTick;

            SetupRoomDataDict();
            SetupInitialRooms();
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
                    roomMap.Add((x, y), RoomFactory.MakeRoom(roomDataDict[RoomType.DBR]));
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
            
        }

        public Sprite GetSpriteByType(RoomType type) {
            return roomDataDict[type].roomSprite;
        }

        public bool IsRoomHovered(int worldX, int worldY)
        {

            return roomMap.ContainsKey(WorldToKey(worldX, worldY));
        }

        private (int, int) WorldToKey(int x, int y)
        {
            return ( (x + 8) / 4 , y / 2 );
        }

        public AbsRoomClickEvent HandleClick(int worldX, int worldY)
        {
            Room room = roomMap[WorldToKey(worldX, worldY)];
            return room.HandleClick();

        }

        public (int, int) WorldGridSnap(int x, int y)
        {
            (int, int) coordinates = WorldToKey(x, y);
            return (4*coordinates.Item1-6, 2*coordinates.Item2+1);
        }

        public Room GetRoom(int x, int y) { 
            return roomMap[WorldToKey(x, y)];
        }
    }
}