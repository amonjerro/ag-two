using UnityEngine;
using UnityEngine.UI;
using Rooms;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    RoomInterface buildInterface;

    [SerializeField]
    GameObject mainInterface;

    [SerializeField]
    NotificationPill pill;


    public void SetRoomToInspect(Room room, (int, int) coordinates)
    {
        buildInterface.SetCoordinates(coordinates);
        buildInterface.SetRoom(room);
    }

    public void ShowBuildInterface()
    {
        // Pause the timer
        ServiceLocator.Instance.GetService<TimeManager>().IsPaused = true;

        // Hide the regular main interface
        mainInterface.SetActive(false);

        // Show the Build Interface
        buildInterface.SetActive(true);
    }

    public void HideBuildInterface() {
        // Unpause the timer
        ServiceLocator.Instance.GetService<TimeManager>().IsPaused = false;

        // Hide the build interface
        buildInterface.SetActive(false);

        // Show the regular main interface
        mainInterface.SetActive(true);
        CameraMovement cam = Camera.main.GetComponent<CameraMovement>();
        cam.Unfocus();
    }

    public void DismissNotifications()
    {
        pill.Dismiss();
    }
}
