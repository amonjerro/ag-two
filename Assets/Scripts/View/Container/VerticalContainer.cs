using UnityEngine;

public class VerticalButtonContainer : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;

    public void ClearContents()
    {
        foreach(Transform child in transform)
        {
            Destroy(child);
        }
    }

    public void AddChild(string buttonText, AbstractCommand command)
    {
        GameObject childObject = Instantiate(prefab, transform);
        CommandButton cb = childObject.GetComponent<CommandButton>();
        cb.SetCommand(command);
        cb.SetText(buttonText);
    }
}