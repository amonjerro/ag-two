namespace Rooms
{
    public static class RoomFactory
    {
        public static Room MakeRoom(RoomData data)
        {
            switch (data.roomType)
            {
                case RoomType.DBR:
                    return new DebrisRoom(data.roomName);
                default:
                    return new OperationsRoom(data.roomName);
            }
        }
    }
}