using UnityEngine;

namespace TileBuilder {
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

        private void SetRectSize()
        {
            rectWidth = rectEndX - rectOriginX;
            rectHeight = rectEndY - rectOriginY;
        }

        public void SetRectEnd(Vector3 rawMouseLocation)
        {
            UpdateTileColors(Color.white);

            // Update locations of the Rect
            Vector3 mouseLocationInWorld = Camera.main.ScreenToWorldPoint(rawMouseLocation);
            int newRectEndX = (int)Mathf.Round(mouseLocationInWorld.x);
            int newRectEndY = (int)Mathf.Round(mouseLocationInWorld.y);

            if (newRectEndX != rectEndX)
            {
                rectEndX = newRectEndX;
            }

            if (newRectEndY != rectEndY)
            {
                rectEndY = newRectEndY;
            }
            SetRectSize();


            UpdateTileColors(Color.green);
        }

        public void BuildRoom()
        {
            int absWidth = Mathf.Abs(rectWidth);
            int signWidth = (int)Mathf.Sign(rectWidth);
            int absHeight = Mathf.Abs(rectHeight);
            int signHeight = (int)Mathf.Sign(rectHeight);

            for (int x = 0; x <= absWidth; x++)
            {
                for (int y = 0; y <= absHeight; y++)
                {
                    int xOffset = x * signWidth;
                    int yOffset = y * signHeight;

                    // We need to paint ground here
                    if (y > 0 && y < absHeight && x > 0 && x < absWidth)
                    {
                        continue;
                    }

                    tileManRef.BuildWall(rectOriginX + xOffset, rectOriginY + yOffset,
                        GetWallFromRectPosition(rectOriginX, rectEndX, rectOriginX + xOffset, rectOriginY, rectEndY, rectOriginY + yOffset)
                    );
                }
            }
        }

        public void UpdateTileColors(Color color)
        {
            int absWidth = Mathf.Abs(rectWidth);
            int signWidth = (int)Mathf.Sign(rectWidth);
            int absHeight = Mathf.Abs(rectHeight);
            int signHeight = (int)Mathf.Sign(rectHeight);

            for (int x = 0; x <= absWidth; x++)
            {
                for (int y = 0; y <= absHeight; y++)
                {
                    int xOffset = x * signWidth;
                    int yOffset = y * signHeight;
                    tileManRef.Paint(rectOriginX + xOffset, rectOriginY + yOffset, color);

                    // We don't paint walls on the insides of the rect
                    if (y > 0 && y < absHeight && x > 0 && x < absWidth)
                    {
                        continue;
                    }

                    // 
                    if (color == Color.white)
                    {
                        tileManRef.UnpaintWall(rectOriginX + xOffset, rectOriginY + yOffset);
                    }
                    else
                    {
                        tileManRef.PaintWall(rectOriginX + xOffset, rectOriginY + yOffset,
                            GetWallFromRectPosition(rectOriginX, rectEndX, rectOriginX + xOffset, rectOriginY, rectEndY, rectOriginY + yOffset)
                        );
                    }
                }
            }
        }



        private WallTypes GetWallFromRectPosition(int originX, int endX, int xPosition, int originY, int endY, int yPosition)
        {

            if (originX == xPosition && originY == yPosition)
            {
                // This is a corner
                if (originX < endX && originY < endY)
                {
                    return WallTypes.BottomLeft;
                }
                else if (originX > endX && originY < endY)
                {
                    return WallTypes.BottomRight;
                }
                else if (originX < endX && originY > endY)
                {
                    return WallTypes.TopLeft;
                }
                else if (originX > endX && originY > endY)
                {
                    return WallTypes.TopRight;
                }

            }
            else if (endX == xPosition && endY == yPosition)
            {
                // This is the opposite corner
                if (originX < endX && originY < endY)
                {
                    return WallTypes.TopRight;
                }
                else if (originX > endX && originY < endY)
                {
                    return WallTypes.TopLeft;
                }
                else if (originX < endX && originY > endY)
                {
                    return WallTypes.BottomRight;
                }
                else if (originX > endX && originY > endY)
                {
                    return WallTypes.BottomLeft;
                }

            }
            else if (xPosition == originX && yPosition == endY)
            {
                // This is a cross corner
                if (originX < endX && originY < endY)
                {
                    return WallTypes.TopLeft;
                }
                else if (originX > endX && originY < endY)
                {
                    return WallTypes.TopRight;
                }
                else if (originX < endX && originY > endY)
                {
                    return WallTypes.BottomLeft;
                }
                else if (originX > endX && originY > endY)
                {
                    return WallTypes.BottomRight;
                }

            }
            else if (endX == xPosition && originY == yPosition)
            {
                // This is a cross corner
                if (originX < endX && originY < endY)
                {
                    return WallTypes.BottomRight;
                }
                else if (originX > endX && originY < endY)
                {
                    return WallTypes.BottomLeft;
                }
                else if (originX < endX && originY > endY)
                {
                    return WallTypes.TopRight;
                }
                else if (originX > endX && originY > endY)
                {
                    return WallTypes.TopLeft;
                }

            }
            else if (xPosition == originX && (yPosition != originY && yPosition != endY))
            {
                // This is the "near" vertical wall
                if (originX < endX)
                {
                    return WallTypes.LeftMid;
                }
                else if (originX > endX)
                {
                    return WallTypes.RightMid;
                }

            }
            else if (xPosition == endX && (yPosition != originY && yPosition != endY))
            {
                // This is the "far" vertical wall
                if (originX < endX)
                {
                    return WallTypes.RightMid;
                }
                else if (originX > endX)
                {
                    return WallTypes.LeftMid;
                }
            }
            else if (yPosition == originY && (xPosition != originX && xPosition != endX))
            {
                // This is the "near" horizontal wall
                if (originY < endY)
                {
                    return WallTypes.BotMid;
                }
                else if (originY > endY)
                {
                    return WallTypes.TopMid;
                }


            }
            else if (yPosition == endY && (xPosition != originX && xPosition != endX))
            {
                // This is the "far" horizontal wall
                if (originY < endY)
                {
                    return WallTypes.TopMid;
                }
                else if (originY > endY)
                {
                    return WallTypes.BotMid;
                }
            }


            return WallTypes.None;
        }

    }
}
