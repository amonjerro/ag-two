using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExplorationMap
{
    // Enums
    public enum ConnectionType
    {
        WILDERNESS = 0,
        ROAD,
        FOREST,
        MOUNTAIN,
        LAKE
    }

    public enum TileStatus
    {
        UNEXPLORED = 0,
        EXPLORED
    }

    public enum ConnectionOrientations
    {
        NORTH = 0,
        EAST,
        SOUTH,
        WEST
    }


    // Base data class to represent the tiles in the exploration map
    public class MapTile
    {
        ConnectionType[] connections = new ConnectionType[4];

        public void SetConnectionType(ConnectionType type, ConnectionOrientations orientation)
        {
            connections[(int)orientation] = type;
        }

        public ConnectionType GetConnectionType(ConnectionOrientations orientation)
        {
            return connections[(int)orientation];
        }

        public MapTile()
        {

        }

        public MapTile(Dictionary<ConnectionOrientations, ConnectionType> connections)
        {
            foreach (KeyValuePair<ConnectionOrientations, ConnectionType> kvp in connections)
            {
                SetConnectionType(kvp.Value, kvp.Key);
            }
        }
    }


    // Base class that holds the data for the entire map, allowing players to explore the world
    public class ExplorationMap
    {
        Dictionary<(int, int), MapTile> tiles;
        Dictionary<(int, int), TileStatus> statusMap;
        public static Action<(int, int)> tileRevealEvent;
        TileMovementSO data;


        public ExplorationMap(TileMovementSO data)
        {
            tiles = new Dictionary<(int, int), MapTile>();
            statusMap = new Dictionary<(int, int), TileStatus>();
            this.data = data;
            data.SetUpTileTypeData();
        }

        private MapTile MakeStartingTile()
        {
            MapTile tile = new MapTile();
            tile.SetConnectionType(ConnectionType.ROAD, ConnectionOrientations.NORTH);
            tile.SetConnectionType(ConnectionType.WILDERNESS, ConnectionOrientations.WEST);
            tile.SetConnectionType(ConnectionType.WILDERNESS, ConnectionOrientations.EAST);
            tile.SetConnectionType(ConnectionType.LAKE, ConnectionOrientations.SOUTH);
            return tile;
        }

        public void InitializeMap(int width, int height)
        {

            for (int x = -width; x <= width; x++) {
                for (int y = -height; y <= height; y++) {
                    if (x == 0 && y == 0)
                    {
                        tiles.Add((0, 0), MakeStartingTile());
                        statusMap.Add((0,0), TileStatus.EXPLORED);
                        continue;
                    }
                    PopulateTile((x, y));
                }
            }
        }

        private MapTile GetNeighborData((int, int) coordinates, ConnectionOrientations orientation)
        {
            (int, int) newCoordinates;
            switch (orientation)
            {
                case ConnectionOrientations.SOUTH:
                    newCoordinates = (coordinates.Item1, coordinates.Item2 + 1);
                    return tiles[newCoordinates];
                case ConnectionOrientations.EAST:
                    newCoordinates = (coordinates.Item1 + 1, coordinates.Item2);
                    return tiles[newCoordinates];
                case ConnectionOrientations.WEST:
                    newCoordinates = (coordinates.Item1 - 1, coordinates.Item2);
                    return tiles[newCoordinates];
                case ConnectionOrientations.NORTH:
                default:
                    newCoordinates = (coordinates.Item1, coordinates.Item2 - 1);
                    return tiles[newCoordinates];
            }
        }

        private TileStatus GetNeighborStatus((int, int) coordinates, ConnectionOrientations orientation)
        {
            (int, int) newCoordinates;
            switch (orientation) {
                case ConnectionOrientations.SOUTH:
                    newCoordinates = (coordinates.Item1, coordinates.Item2+1);
                    if (statusMap.ContainsKey(newCoordinates))
                    {
                        return statusMap[newCoordinates];
                    }
                    PopulateTile(newCoordinates);
                    return TileStatus.UNEXPLORED;
                case ConnectionOrientations.EAST:
                    newCoordinates = (coordinates.Item1 + 1, coordinates.Item2);
                    if (statusMap.ContainsKey(newCoordinates))
                    {
                        return statusMap[newCoordinates];
                    }
                    PopulateTile(newCoordinates);
                    return TileStatus.UNEXPLORED;
                case ConnectionOrientations.WEST:
                    newCoordinates = (coordinates.Item1-1, coordinates.Item2);
                    if (statusMap.ContainsKey(newCoordinates))
                    {
                        return statusMap[newCoordinates];
                    }
                    PopulateTile(newCoordinates);
                    return TileStatus.UNEXPLORED;
                case ConnectionOrientations.NORTH:
                default:
                    newCoordinates = (coordinates.Item1, coordinates.Item2 - 1);
                    if (statusMap.ContainsKey(newCoordinates)){
                        return statusMap[newCoordinates];
                    }
                    PopulateTile(newCoordinates);
                    return TileStatus.UNEXPLORED;
            } 
        }

        private void PopulateTile((int,  int) coordinates)
        {
            statusMap.Add(coordinates, TileStatus.UNEXPLORED);
            tiles.Add(coordinates, new MapTile());
        }

        private ConnectionOrientations GetOppositeOrientation(ConnectionOrientations orientation)
        {
            switch (orientation)
            {
                case ConnectionOrientations.NORTH:
                    return ConnectionOrientations.SOUTH;
                case ConnectionOrientations.SOUTH:
                    return ConnectionOrientations.NORTH;
                case ConnectionOrientations.EAST:
                    return ConnectionOrientations.WEST;
                case ConnectionOrientations.WEST:
                    return ConnectionOrientations.EAST;
                default:
                    throw new ArgumentException("This orientation is not supported");
            }
        }

        private ConnectionType GetRandomConnectionType()
        {
            float roll = UnityEngine.Random.Range(0.0f, 1.0f);
            if (roll <= data.GetOdds(ConnectionType.WILDERNESS)) {
                return ConnectionType.WILDERNESS;
            } else if (roll <= data.GetOdds(ConnectionType.ROAD))
            {
                return ConnectionType.ROAD;
            } else if  (roll <= data.GetOdds(ConnectionType.FOREST)){
                return ConnectionType.FOREST;
            } else if (roll <= data.GetOdds(ConnectionType.ROAD))
            {
                return ConnectionType.MOUNTAIN;
            }
            return ConnectionType.LAKE;

        }

        private void SetTileDataFromNeighborConnection(MapTile tile, (int, int) coordinates, ConnectionOrientations orientation)
        {
            // If a neighbor has already been explored, match their connection
            if (GetNeighborStatus(coordinates, orientation) == TileStatus.EXPLORED)
            {
                tile.SetConnectionType(GetNeighborData(coordinates, orientation).GetConnectionType(GetOppositeOrientation(orientation)), orientation);
                return;
            }

            // Pick a random connection type
            // This can be significantly expanded
            tile.SetConnectionType(GetRandomConnectionType(), orientation);

        }

        public void UncoverTile((int, int) coordinates)
        {
            statusMap[coordinates] = TileStatus.EXPLORED;
            MapTile tile = tiles[coordinates];

            // Set the tile connections //
            SetTileDataFromNeighborConnection(tile, coordinates, ConnectionOrientations.NORTH);
            SetTileDataFromNeighborConnection(tile, coordinates, ConnectionOrientations.EAST);
            SetTileDataFromNeighborConnection(tile, coordinates, ConnectionOrientations.WEST);
            SetTileDataFromNeighborConnection(tile, coordinates, ConnectionOrientations.SOUTH);

            // Call observers
            tileRevealEvent?.Invoke(coordinates);
        }

        public void CreateNamedTile((int, int) coordinates, Dictionary<ConnectionOrientations, ConnectionType> connections)
        {
            tiles[coordinates] = new MapTile(connections);
            statusMap[coordinates] = TileStatus.EXPLORED;

            tileRevealEvent?.Invoke(coordinates);
        }

        public MapTile GetTile((int, int) coordinates) { 
            return tiles[coordinates];
        }

        public TileStatus GetTileStatus((int, int) coordinates)
        {
            return statusMap[coordinates];
        }

        public bool TileIsExplorable((int, int) coordinates) {
            return statusMap[(coordinates.Item1 - 1, coordinates.Item2)] == TileStatus.EXPLORED || 
                statusMap[(coordinates.Item1 + 1, coordinates.Item2)] == TileStatus.EXPLORED ||
                statusMap[(coordinates.Item1, coordinates.Item2-1)] == TileStatus.EXPLORED || 
                statusMap[(coordinates.Item1, coordinates.Item2+1)] == TileStatus.EXPLORED;
        }
    }
}