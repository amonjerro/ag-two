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
        public static int tickCount = 14;
    }
}
