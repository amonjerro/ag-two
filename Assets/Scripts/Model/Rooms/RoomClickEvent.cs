namespace Rooms
{
    public enum RoomClickEventTypes {
        SceneTransition,
        MenuOpen
    }

    public abstract class AbsRoomClickEvent
    {
        public RoomType roomType;
        public RoomClickEventTypes type;
        public abstract int GetValue();
    }


    public class SceneTransitionEvent : AbsRoomClickEvent
    {
        
        public SceneTransitionEvent(RoomType room) {
            type = RoomClickEventTypes.SceneTransition;
            roomType = room;
        }

        public override int GetValue()
        {
            return Constants.MapRoomTypeToInt(roomType);
        }
    }

    public class MenuOpenEvent : AbsRoomClickEvent
    {

        public MenuOpenEvent(RoomType room)
        {
            roomType = room;
            type = RoomClickEventTypes.MenuOpen;
        }

        public override int GetValue()
        {
            return Constants.MapRoomTypeToInt(roomType);
        }
    }
}