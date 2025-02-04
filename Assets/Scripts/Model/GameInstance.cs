using Rooms;
using Tasks;
using System.Collections.Generic;
using ExplorationMap;
using System;

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

        public static List<Task> activeTasks = new List<Task>();

        public static Action tasksUpdated;

        public static bool TestWalkability(ConnectionType connectionType)
        {
            return false;
        }

        public static void SortTasks()
        {
            activeTasks.Sort(delegate (Task t1, Task t2){ return t1.RemainingTime.CompareTo(t2.RemainingTime); });
        }


    }
}
