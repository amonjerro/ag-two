using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Tileset")]
public class TileGroup : ScriptableObject
{
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private int size;


    public int GetSize() { return size; }
    public Sprite GetRandomSprite(int index)
    {
        int sampleIndex = index;
        if (index > sprites.Length)
        {
            sampleIndex = index % sprites.Length;
        }

        return sprites[sampleIndex];
    }
}
