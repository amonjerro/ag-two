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
}
