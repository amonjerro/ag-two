using TMPro;
using Rooms;
using Tasks;

public class ShowRoomDetailCommand : AbstractCommand
{
    TextMeshProUGUI descriptionText;
    TextMeshProUGUI titleText;

    Room room;

    public ShowRoomDetailCommand(TextMeshProUGUI title, TextMeshProUGUI desc, Room room) { 
        titleText = title;
        descriptionText = desc;
        this.room = room;
    }

    public override void Execute()
    {
        descriptionText.text = room.Description;
        titleText.text = room.Name;
    }
}

public class BuildCommand : AbstractCommand
{
    (int, int) roomCoordinates;
    RoomType roomType;

    public BuildCommand((int, int) roomCoordinates, RoomType buildType)
    {
        this.roomCoordinates = roomCoordinates;
        this.roomType = buildType;
    }

    public override void Execute()
    {
        RoomManager roomManager = ServiceLocator.Instance.GetService<RoomManager>();
        roomManager.BeginRoomBuild(roomCoordinates.Item1, roomCoordinates.Item2, roomType);
    }
}

public class NotifyAcknowledge : AbstractCommand
{
    TaskNotify notify;
    public NotifyAcknowledge(TaskNotify taskNotify)
    {
        notify = taskNotify;
    }
    public override void Execute()
    {
        UIManager uiManager = ServiceLocator.Instance.GetService<UIManager>();
        notify.Acknowledge();
        uiManager.DismissNotifications();
    }
}