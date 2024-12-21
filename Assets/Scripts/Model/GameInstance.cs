using Rooms;
using System.Collections.Generic;

namespace SaveGame
{
    [System.Serializable]
    public static class GameInstance
    {
        public static List<Adventurer> roster;
        public static List<QuestData> completedQuests;
        public static List<RoomSaveData> savedRoomData;

        public static int gold = 100;
        public static int totalTickCount = 14;
    }
}
