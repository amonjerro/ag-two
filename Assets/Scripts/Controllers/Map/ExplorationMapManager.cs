using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ExplorationMap
{
    public class ExplorationMapManager : MonoBehaviour
    {
        [SerializeField]
        private int mapHeight;
        
        [SerializeField]
        private int mapWidth;

        ExplorationMap explorationMap;

        [SerializeField]
        TileMovementSO mapTileData;

        [SerializeField]
        MapTileSpritesSO mapTileSprites;

        [SerializeField]
        GameObject mapViewPrefab;

        [SerializeField]
        GameObject mapHierarchy;

        private int currentX;
        private int currentY;

        private ConnectionOrientations[] orientationIterator = { ConnectionOrientations.NORTH, ConnectionOrientations.EAST, ConnectionOrientations.SOUTH, ConnectionOrientations.WEST };

        private void Awake()
        {
            explorationMap = new ExplorationMap(mapTileData);
            mapTileSprites.SetUpData();
            currentX = 0;
            currentY = 0;
            
        }
        private void Start()
        {
            CreateViewItems();
            explorationMap.InitializeMap(mapWidth, mapHeight);
        }

        private void MoveMap()
        {

        }

        private void Update()
        {
            
        }

        private void OnMove(InputValue value)
        {

        }

        private void OnSelect(InputValue value) {
            Mouse mouse = Mouse.current;
            Vector3 mousePosition = new Vector3(mouse.position.x.value, mouse.position.y.value, 0);
            Vector3 realWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition + new Vector3(0,0,10));

            // Adjust the position by how much the map viewport has moved
            int positionX = Mathf.FloorToInt(realWorldPosition.x) + currentX;
            int positionY = Mathf.FloorToInt(realWorldPosition.y) + currentY;

            MapClickEvent clickEvent = new MapClickEvent();
            clickEvent.Coordinates = (positionX, positionY);
            clickEvent.TileStatus = explorationMap.GetTileStatus((positionX, positionY));
            clickEvent.IsExplorable = explorationMap.TileIsExplorable((positionX, positionY));
        }

        public ExplorationMap GetMapReference() { return explorationMap; }

        public List<Sprite> GetSpritesForMapTile((int, int) coordinates)
        {
            List<Sprite> sprites = new List<Sprite>();
            Sprite toAdd;

            sprites.Add(mapTileSprites.baseSprite);
            MapTile tile = explorationMap.GetTile(coordinates);
            foreach(ConnectionOrientations co in orientationIterator)
            {
                toAdd = mapTileSprites.GetByTypeAndOrientation(tile.GetConnectionType(co), co);
                if (toAdd != null)
                {
                    sprites.Add(toAdd);
                }
            }

            return sprites;
        }

        private void CreateViewItems()
        {
            float startX = -mapWidth + 0.5f;
            float startY = -mapHeight + 0.5f;
            for (int x = 0; x < mapWidth*2+1; x++) {
                for (int y = 0; y < mapHeight*2+1; y++) { 
                    GameObject go = Instantiate(mapViewPrefab, new Vector3(x + startX, y + startY, 0), Quaternion.identity);
                    go.transform.SetParent(mapHierarchy.transform, true);
                }
            }
        }
    }
}