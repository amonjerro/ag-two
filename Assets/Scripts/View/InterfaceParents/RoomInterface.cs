using Rooms;

public class RoomInterface : InterfaceCollection
{
    Room room;

    public void SetRoom(Room room)
    {
        this.room = room;
    }

    public string GetRoomTitle()
    {
        return room.Name;
    }
}