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
    Dictionary<Vector2, TileTypes> groundMap;
    Dictionary<Vector2, TileTypes> buildingMap;

    public TileMap(int size_x, int size_y)
    {
        groundMap = new Dictionary<Vector2, TileTypes> ();
        buildingMap = new Dictionary<Vector2, TileTypes> ();

        flywheel = new Dictionary<TileTypes, TileData> ();
        for (int y = -size_y; y < size_y; y++) {
            for (int x = -size_x; x < size_x; x++) {
                Vector2 coordinates = new Vector2(x, y);
                groundMap.Add(coordinates, TileTypes.Grass);
            }
        }


        foreach (var value in Enum.GetValues(typeof(TileTypes))) {
            flywheel.Add((TileTypes)value, MakeTile((TileTypes)value));
        }
    }

    public TileTypes LookupType(Vector2 coordinates)
    {
        return groundMap[coordinates];
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

    public void AddBuilding(Vector2 location, TileTypes type)
    {
        buildingMap.Add(location, type);
    }
}