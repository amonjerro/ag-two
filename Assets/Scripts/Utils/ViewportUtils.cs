using UnityEngine;

public static class ViewportUtils
{

    public static float GetDistanceFromRight(Vector2 mousePosition)
    {
        return Screen.width - mousePosition.x;
    }

}