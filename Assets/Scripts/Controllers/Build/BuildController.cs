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


    public void SetRectOrigin(int x, int y)
    {
        rectOriginX = x; 
        rectOriginY = y;
    }

    private void SetRectSize(int width, int height) {
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

    public void SetRectEnd(int x, int y) { 
        rectEndX = x;
        rectEndY = y;
    }

    public void BuildRoom()
    {
        for (int y = rectOriginY; y < rectHeight; y++) {
            for (int x = rectOriginX; x < rectWidth; x++) { 
                
            }
        }
    }

}