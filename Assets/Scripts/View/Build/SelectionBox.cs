using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectionBox : MonoBehaviour
{
    RectTransform rt;
    Image image;
    bool showing;

    float originX;
    float originY;
    Vector2 currentMousePosition;

    public void Awake()
    {
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        showing = false;
        rt.sizeDelta = Vector2.zero;
    }

    public void Update()
    {
        if (!showing)
        {
            return;
        }

        currentMousePosition = Mouse.current.position.ReadValue();
        float deltaX = currentMousePosition.x - originX;
        float deltaY = currentMousePosition.y - originY;
        rt.sizeDelta = new Vector2(Mathf.Abs(deltaX), Mathf.Abs(deltaY));
        rt.anchoredPosition = new Vector2(originX, originY) + new Vector2(deltaX * 0.5f, deltaY * 0.5f);
    }

    public void Show()
    {
        showing = true;
    }

    public void Hide()
    {
        showing = false;
        rt.sizeDelta = Vector2.zero;
    }

    public void SetOrigin(float x, float y)
    {
        originX = x;
        originY = y;
    }
    
}