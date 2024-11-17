using UnityEngine;

public class BuildController
{
    TileManager tileManRef;
    
    public BuildController() { }
    private int rectOriginX;
    private int rectOriginY;
    private int rectEndY;
    private int rectEndX;
    private int rectWidth;
    private int rectHeight;


    public void SetTileManagerReference(TileManager tileManRef)
    {
        this.tileManRef = tileManRef;
    }

    public void SetRectOrigin(Vector3 rawMouseLocation)
    {
        Vector3 mouseLocationInWorld = Camera.main.ScreenToWorldPoint(rawMouseLocation);
        rectOriginX = (int)mouseLocationInWorld.x; 
        rectOriginY = (int)mouseLocationInWorld.y;
    }

    private void SetRectSize() {
        int temp;
        if (rectEndX < rectOriginX)
        {
            temp = rectEndX;
            rectEndX = rectOriginX;
            rectOriginX = temp;
        }

        if (rectEndY < rectOriginY)
        {
            temp = rectEndY;
            rectEndY = rectOriginY;
            rectOriginY = temp;
        }

        rectWidth = rectEndX - rectOriginX;
        rectHeight = rectEndY - rectOriginY;
    }

    public void SetRectEnd(Vector3 rawMouseLocation) {
        Vector3 mouseLocationInWorld = Camera.main.ScreenToWorldPoint(rawMouseLocation);
        rectEndX = (int) mouseLocationInWorld.x;
        rectEndY = (int) mouseLocationInWorld.y;
        SetRectSize();
        UpdateTileColors(Color.green);
    }

    public void BuildRoom()
    {
        for (int y = rectOriginY; y < rectHeight; y++) {
            for (int x = rectOriginX; x < rectWidth; x++) { 
                
            }
        }
    }

    public void UpdateTileColors(Color color)
    {
        int absWidth = Mathf.Abs(rectWidth);
        int signWidth = (int) Mathf.Sign(rectWidth);
        int absHeight = Mathf.Abs(rectHeight);
        int signHeight = (int)Mathf.Sign(rectHeight);
        if (color == Color.white) {
            Debug.Log(absWidth);
            Debug.Log(rectOriginX);
        }
        for (int x = 0; x <= absWidth; x++)
        {
            for (int y = 0;  y <= absHeight; y++)
            {
                tileManRef.Paint(rectOriginX + x * signWidth, rectOriginY + y * signHeight, color);
            }
        }
    }

}