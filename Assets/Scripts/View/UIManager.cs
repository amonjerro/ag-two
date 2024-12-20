using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject buildInterface;

    [SerializeField]
    GameObject mainInterface;

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
}
