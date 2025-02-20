using Rooms;

public static class Constants
{
    public const string ScriptableObjectsPath = "ScriptableObjects/";
    public const string QuestChooseAdventurers = "Choose Party";
    public const string QuestClose = "Close";

    public static int MapRoomTypeToInt(RoomType type)
    {
        switch (type)
        {
            case RoomType.OPS:
                return 2;
            default:
                return 1;
        }
    }

    public static int MapComponentTypeToUIPanelInt(ComponentType type)
    {
        switch (type)
        {
            case ComponentType.BUILDABLE:
                return 1;
            default:
                return 0;
        }
        
    }
} 