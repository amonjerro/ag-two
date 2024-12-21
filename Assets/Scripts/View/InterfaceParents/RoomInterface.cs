using Rooms;

public class RoomInterface : InterfaceCollection
{
    Room room;
    (int, int) roomCoordinates;

    public void SetCoordinates((int, int) roomCoordinates)
    {
        this.roomCoordinates = roomCoordinates;
    }

    public void SetRoom(Room room)
    {
        this.room = room;
    }

    public string GetRoomTitle()
    {
        return room.Name;
    }

    public RoomType GetSelectedRoomType()
    {
        return room.roomType;
    }

    public (int, int) GetRoomCoordinates() { return roomCoordinates; }

    public RoomComponent GetRoomComponent(ComponentType compoType) { return room.GetRoomComponent(compoType); }
}