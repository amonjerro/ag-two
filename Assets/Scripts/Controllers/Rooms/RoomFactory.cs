namespace Rooms
{
    public static class RoomFactory
    {
        public static Room MakeRoom(RoomData data)
        {
            RoomBuilder roomBuilder = new RoomBuilder();

            switch (data.roomType) {
                case RoomType.LIB:
                    roomBuilder.Reset(new LibraryRoom());
                    break;
                case RoomType.BRK:
                    roomBuilder.Reset(new Barracks());
                    break;
                case RoomType.DBR:
                    roomBuilder.Reset(new DebrisRoom());
                    break;
                default:
                    roomBuilder.Reset(new OperationsRoom());
                    break;
            }

            roomBuilder.SetName(data.roomName);
            roomBuilder.SetDescription(data.roomDescription);
            for (int i = 0; i < data.componentData.Count; i++)
            {
                roomBuilder.AddComponent(MakeRoomComponent(data.componentData[i]));
            }
            return roomBuilder.GetResult();
        }

        public static RoomComponent MakeRoomComponent(ComponentData data) {
            switch (data.componentType)
            {
                case ComponentType.HOLDS_ADVENTURERS:
                    return new HoldComponent(data.cost, data.upgradeCount, data.upgradeCostFactor);
                case ComponentType.UPGRADEABLE:
                    return new UpgradeableComponent(data.cost, data.upgradeCount, data.upgradeCostFactor);
                default:
                    return new BuildableComponent(data.cost, data.upgradeCostFactor);
            }
        }
    }

    public class RoomBuilder
    {

        Room room;
        public void Reset(Room room)
        {
            this.room = room;
        }

        public Room GetResult()
        {
            return room;
        }

        public void AddComponent(RoomComponent component)
        {
            room.AddComponent(component);
        }

        public void SetName(string name)
        {
            room.Name = name;
        }

        public void SetDescription(string description) { 
            room.Description = description;
        }
    }
}