using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectionBox : MonoBehaviour
{
    RectTransform rt;
    Image image;
    bool showing;

    Vector2 origin;
    Vector2 currentMousePosition;

    public void Awake()
    {
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        showing = false;
    }

    public void Update()
    {
        if (!showing)
        {
            return;
        }

        currentMousePosition = Mouse.current.position.ReadValue();
        float deltaX = currentMousePosition.x - origin.x;
        float deltaY = currentMousePosition.y - origin.y;
        rt.sizeDelta = new Vector2(Mathf.Abs(deltaX), Mathf.Abs(deltaY));
        rt.anchoredPosition = new Vector2(origin.x, origin.y) + new Vector2(deltaX * 0.5f, deltaY * 0.5f);
    }

    public void ToggleVisibility()
    {
        showing = !showing;
    }
    
}