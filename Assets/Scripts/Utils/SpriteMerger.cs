using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class SpriteMerger
{
    public static Sprite MergeSprites(List<Sprite> sprites)
    {
        Texture2D targetTexture = new Texture2D(32, 32, TextureFormat.RGBA32, false, false);
        targetTexture.filterMode = FilterMode.Point;
        for (int x = 0; x < targetTexture.width; x++)
        {
            for (int y = 0; y < targetTexture.height; y++)
            {
                targetTexture.SetPixel(x, y, new Color(1, 1, 1, 0));
            }
        }

        foreach (Sprite sprite in sprites)
        {
            for (int x = 0; x < sprite.texture.width; x++)
            {
                for (int y = 0; y < sprite.texture.height; y++)
                {

                    Color color = sprite.texture.GetPixel(x, y).a == 0 ? targetTexture.GetPixel(x, y) : sprite.texture.GetPixel(x, y);
                    targetTexture.SetPixel(x,y, color);
                }
            }
        }
        targetTexture.Apply();
        Sprite finalSprite = Sprite.Create(targetTexture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f));
        return finalSprite;
    }
}
