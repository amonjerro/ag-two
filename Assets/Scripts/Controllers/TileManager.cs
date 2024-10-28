using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject tileContainer;

    [SerializeField]
    private int mapSize_x;

    [SerializeField]
    private int mapSize_y;

    [SerializeField]
    private TileGroup grassTiles;

    [SerializeField]
    private TileGroup wallTiles;

    private TileMap tilemap;
    
    // Start is called before the first frame update
    void Start()
    {
        CreateDefaultMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateDefaultMap()
    {
        tilemap = new TileMap(mapSize_x, mapSize_y);
        for (int y = -mapSize_y; y < mapSize_y; y++)
        {
            for (int x = -mapSize_x; x < mapSize_x; x++)
            {
                GameObject go = new GameObject();
                go.name = "Tile_" + x + "_" + y;
                go.transform.parent = tileContainer.transform;
                go.transform.position = new Vector3(x, y, 0);

                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = grassTiles.GetRandomSprite(Random.Range(0, grassTiles.GetSize()));
            }
        }
    }

    public void BuildWall(int x, int y, WallTypes type)
    {
        Vector2 location = new Vector2(x, y);
        Sprite sprite;
        switch (type) {
            case WallTypes.TopLeft:
                sprite = wallTiles.GetSprite(0);
                break;
            case WallTypes.TopMid:
                sprite = wallTiles.GetSprite(1);
                break;
            case WallTypes.TopRight:
                sprite = wallTiles.GetSprite(2);
                break;
            case WallTypes.LeftMid:
                sprite = wallTiles.GetSprite(3);
                break;
            case WallTypes.RightMid:
                sprite = wallTiles.GetSprite(4);
                break;
            case WallTypes.BottomLeft:
                sprite = wallTiles.GetSprite(5);
                break;
            case WallTypes.BotMid:
                sprite = wallTiles.GetSprite(6);
                break;
            case WallTypes.BottomRight:
                sprite = wallTiles.GetSprite(7);
                break;
            default:
                sprite = wallTiles.GetSprite(0);
                break;
        }

        tilemap.AddBuilding(location, TileTypes.Wall);
        GameObject gameObject = new GameObject();
        gameObject.name = "Wall_" + x + "_" + y;
        gameObject.transform.parent = tileContainer.transform;
        gameObject.transform.position = new Vector3(x, y, 0);
        SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
    }
}
