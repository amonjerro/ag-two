using TMPro;
using Rooms;

public class ShowRoomDetailCommand : AbstractCommand
{
    TextMeshProUGUI descriptionText;
    TextMeshProUGUI titleText;

    Room room;

    public ShowRoomDetailCommand(TextMeshProUGUI title, TextMeshProUGUI desc, Room room) { 
        titleText = title;
        descriptionText = desc;
    }

    public override void Execute()
    {
        descriptionText.text = room.Description;
        titleText.text = room.Name;
    }
}