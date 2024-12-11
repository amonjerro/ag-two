using Tasks;
namespace Rooms
{
    [System.Serializable]
    public class RoomSaveData
    {
        int positionX;
        int positionY;
        int roomType;
        TaskSaveData activeTask;
    }
}