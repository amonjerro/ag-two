using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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

    private Dictionary<(int x, int y), SpriteRenderer> groundSpriteRenderers;
    private Dictionary<(int x, int y), SpriteRenderer> ghostSpriteRenderers;
    
    // Start is called before the first frame update
    void Start()
    {
        groundSpriteRenderers = new Dictionary<(int x, int y), SpriteRenderer>();
        ghostSpriteRenderers  = new Dictionary<(int x, int y), SpriteRenderer>();
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
                // The real thing
                GameObject go = new GameObject();
                go.name = "Tile_" + x + "_" + y;
                go.transform.parent = tileContainer.transform;
                go.transform.position = new Vector3(x, y, 0);

                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = grassTiles.GetRandomSprite(UnityEngine.Random.Range(0, grassTiles.GetSize()));
                groundSpriteRenderers.Add((x, y), sr);

                // The real thing
                GameObject ghostObject = new GameObject();
                ghostObject.name = "GhostTile_" + x + "_" + y;
                ghostObject.transform.parent = tileContainer.transform;
                ghostObject.transform.position = new Vector3(x, y, 0);

                SpriteRenderer ghostSR = ghostObject.AddComponent<SpriteRenderer>();
                ghostSpriteRenderers.Add((x, y), ghostSR);
            }
        }
    }

    public void Paint(int x, int y, Color color)
    {
        SpriteRenderer sr;
        groundSpriteRenderers.TryGetValue((x, y), out sr);
        if (sr == null) {
            return;
        }

        sr.color = color;
    }

    private Sprite PickWallSprite(WallTypes type) 
    {
        Sprite sprite;
        switch (type)
        {
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
                sprite = null;
                break;
        }
        return sprite;
    }

    public void BuildWall(int x, int y, WallTypes type)
    {
        Vector2 location = new Vector2(x, y);
        tilemap.AddBuilding(location, TileTypes.Wall);
        GameObject gameObject = new GameObject();
        gameObject.name = "Wall_" + x + "_" + y;
        gameObject.transform.parent = tileContainer.transform;
        gameObject.transform.position = new Vector3(x, y, 0);
        SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = PickWallSprite(type);
    }

    public void PaintWall(int x, int y, WallTypes type)
    {
        SpriteRenderer ghost = ghostSpriteRenderers[(x, y)];
        ghost.sprite = PickWallSprite(type);
    }

    public void UnpaintWall(int x, int y)
    {
        SpriteRenderer ghost = ghostSpriteRenderers[(x, y)];
        ghost.sprite = null;
    }
}
