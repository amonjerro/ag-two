using System.Collections.Generic;
using UnityEngine;

namespace Rooms
{
    public class RoomManager : MonoBehaviour
    {
        Dictionary<(int, int), Room> roomMap;
        public List<Room> availableRooms;
        public List<RoomData> roomData;
        private Dictionary<RoomType, RoomData> roomDataDict;

        [SerializeField]
        int fortressWidth;

        [SerializeField]
        int fortressHeight;

        private void Awake()
        {
            roomMap = new Dictionary<(int, int), Room>();
            roomDataDict = new Dictionary<RoomType, RoomData> ();
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
            int localX = (worldX + 6) / 4;
            int localY = (worldY - 1) / 2;

            return roomMap.ContainsKey((localX, localY));
        }
}
}