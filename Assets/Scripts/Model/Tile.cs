using System;
using System.Collections.Generic;
using UnityEngine;

public enum TileTypes
{
    Grass,
    Floor,
    Wall
}

public class TileData
{
    public bool Walkable { get; private set; }

    public TileData (bool walkable)
    {
        Walkable = walkable;
    }

}

public class TileMap
{
    Dictionary<TileTypes, TileData> flywheel;
    Dictionary<Vector2, TileTypes> map;

    public TileMap(int size_x, int size_y)
    {
        map = new Dictionary<Vector2, TileTypes> ();
        flywheel = new Dictionary<TileTypes, TileData> ();
        for (int y = -size_y; y < size_y; y++) {
            for (int x = -size_x; x < size_x; x++) {
                Vector2 coordinates = new Vector2(x, y);
                map.Add(coordinates, TileTypes.Grass);
            }
        }


        foreach (var value in Enum.GetValues(typeof(TileTypes))) {
            flywheel.Add((TileTypes)value, MakeTile((TileTypes)value));
        }
    }

    public TileTypes LookupType(Vector2 coordinates)
    {
        return map[coordinates];
    }

    public TileData LookupData(TileTypes type) { return flywheel[type]; }

    private TileData MakeTile(TileTypes type)
    {
        switch (type) {
            case TileTypes.Wall:
                return new TileData(false);
            case TileTypes.Floor:
                return new TileData(true);
            default:
                return new TileData(true);

        }
    }
}