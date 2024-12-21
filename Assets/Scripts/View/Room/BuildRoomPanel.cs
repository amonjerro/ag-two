
using System.Collections.Generic;
using UnityEngine;
using Rooms;
using TMPro;
using SaveGame;

public class BuildRoomPanel : UIPanel
{
    [SerializeField]
    private VerticalButtonContainer availableRoomsContainer;

    [SerializeField]
    TextMeshProUGUI titleField;

    [SerializeField]
    TextMeshProUGUI descriptionField;

    [SerializeField]
    CommandButton buildButton;

    public override void Show()
    {
        RoomInterface parentInterface = GetComponentInParent<RoomInterface>();
        (int, int) coordinates = parentInterface.GetRoomCoordinates();
        BuildRestrictions restriction = BuildRestrictions.NONE;

        if (coordinates.Item2 < 0)
        {
            restriction = BuildRestrictions.ABOVE_GROUND;
        } else
        {
            restriction = BuildRestrictions.UNDERGROUND;
        }

        List<Room> availableRooms = ServiceLocator.Instance.GetService<RoomManager>().FilterAvailableRooms(restriction);
        foreach (Room room in availableRooms) {
            ShowRoomDetailCommand showRoom = new ShowRoomDetailCommand(titleField, descriptionField, room);
            availableRoomsContainer.AddChild(room.Name, showRoom);
        }

        BuildCommand buildCommand = new BuildCommand(coordinates, parentInterface.GetSelectedRoomType());
        buildButton.SetButtonEnabled(false);
        buildButton.SetCommand(buildCommand);

        RoomComponent component = parentInterface.GetRoomComponent(ComponentType.BUILDABLE);
        if (component != null)
        {
            if (component.GetCost() <= GameInstance.gold)
            {
                buildButton.SetButtonEnabled(true);
            }
        }
        
        
    }

    public override void Dismiss()
    {
        
    }
}