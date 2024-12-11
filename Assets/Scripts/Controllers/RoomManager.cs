using System.Collections.Generic;
using UnityEngine;

namespace Rooms
{
    public class RoomManager : MonoBehaviour
    {
        Dictionary<(int, int), Room> roomMap;
        public List<Room> availableRooms;

        private void Start()
        {
            roomMap = new Dictionary<(int, int), Room>();
            TimeManager.Tick += HandleTick;
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
    }

}