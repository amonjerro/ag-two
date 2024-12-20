
using System.Collections.Generic;
using UnityEngine;
using Rooms;
using TMPro;

public class BuildRoomPanel : UIPanel
{
    [SerializeField]
    private VerticalButtonContainer availableRoomsContainer;

    [SerializeField]
    TextMeshProUGUI titleField;

    [SerializeField]
    TextMeshProUGUI descriptionField;

    public override void Show()
    {
        List<Room> availableRooms = ServiceLocator.Instance.GetService<RoomManager>().availableRooms;
        foreach (Room room in availableRooms) {
            ShowRoomDetailCommand showRoom = new ShowRoomDetailCommand(titleField, descriptionField, room);
            availableRoomsContainer.AddChild(room.Name, showRoom);
        }

    }

    public override void Dismiss()
    {
        
    }
}