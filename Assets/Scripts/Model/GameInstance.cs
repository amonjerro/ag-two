using Rooms;
using Tasks;
using System.Collections.Generic;
using ExplorationMap;

namespace SaveGame
{
    [System.Serializable]
    public static class GameInstance
    {
        public static List<Adventurer> roster = new List<Adventurer>();
        public static List<QuestData> completedQuests = new List<QuestData>();
        public static List<RoomSaveData> savedRoomData = new List<RoomSaveData>();

        public static int gold = 100;
        public static int totalTickCount = 14;

        public static Queue<Task> tasksToPopulate = new Queue<Task>();
        public static List<(int, int)> exploredCoordinates;

        public static bool TestWalkability(ConnectionType connectionType)
        {
            return false;
        }


    }
}
