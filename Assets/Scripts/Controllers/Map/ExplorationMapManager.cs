using SaveGame;
using System.Collections.Generic;
using System.Linq;
using Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ExplorationMap
{
    public enum ExplorationCursorState
    {
        Selecting,
        GUIInteraction
    }

    public class ExplorationMapManager : MonoBehaviour
    {
        private ExplorationCursorState _cursorState;
        private MapClickEvent _latestEvent;

        [SerializeField]
        private int mapHeight;
        
        [SerializeField]
        private int mapWidth;

        MapUIManager mapUIManager;

        ExplorationMap explorationMap;

        [SerializeField]
        TileMovementSO mapTileData;

        [SerializeField]
        MapTileSpritesSO mapTileSprites;

        [SerializeField]
        GameObject mapViewPrefab;

        [SerializeField]
        GameObject mapHierarchy;

        [SerializeField]
        GameObject embarkHierarchy;

        private int currentX;
        private int currentY;

        ExplorationTask explorationTask;
        QuestTask questTask;

        private ConnectionOrientations[] orientationIterator = { ConnectionOrientations.NORTH, ConnectionOrientations.EAST, ConnectionOrientations.SOUTH, ConnectionOrientations.WEST };

        private void Awake()
        {
            explorationMap = new ExplorationMap(mapTileData);
            mapUIManager = GetComponent<MapUIManager>();
            mapTileSprites.SetUpData();
            currentX = 0;
            currentY = 0;
            
        }
        private void Start()
        {
            _cursorState = ExplorationCursorState.Selecting;
            CreateViewItems();
            explorationTask = new ExplorationTask();
            questTask = new QuestTask();

            explorationMap.InitializeMap(mapWidth, mapHeight);
            for(int i = 0; i < 4; i++)
            {
                int candidateX = (2 - i) * (i % 2);
                int candidateY = (1 - i) * ((1 + i) % 2);
                explorationMap.UncoverTile((candidateX, candidateY));
            }
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
            switch (_cursorState)
            {
                case ExplorationCursorState.GUIInteraction:
                    break;
                default:
                    HandleMapSelect();
                    break;
            }
        }

        private void CreateMapClickEvent(int positionX, int positionY)
        {
            MapClickEvent clickEvent = new MapClickEvent();
            clickEvent.Coordinates = (positionX, positionY);
            explorationTask.SetCoordinates(clickEvent.Coordinates);

            clickEvent.TileStatus = explorationMap.GetTileStatus((positionX, positionY));
            clickEvent.IsExplorable = explorationMap.TileIsExplorable((positionX, positionY));

            if (explorationMap.GetTraversalCost(clickEvent.Coordinates) == -1)
            {
                Debug.LogError("No path found!!!");
            }

            _latestEvent = clickEvent;
            if (mapUIManager.WillShowGUI(clickEvent))
            {
                mapUIManager.HandleMapClickEvent(clickEvent);
                _cursorState = ExplorationCursorState.GUIInteraction;
            }
        }

        private void HandleMapSelect()
        {
            Mouse mouse = Mouse.current;
            Vector3 mousePosition = new Vector3(mouse.position.x.value, mouse.position.y.value, 0);
            Vector3 realWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition + new Vector3(0, 0, 10));

            // Adjust the position by how much the map viewport has moved
            int positionX = Mathf.FloorToInt(realWorldPosition.x) + currentX;
            int positionY = Mathf.FloorToInt(realWorldPosition.y) + currentY;

            CreateMapClickEvent(positionX, positionY);
        }

        public ExplorationMap GetMapReference() { return explorationMap; }
        public void ResetCursor()
        {
            _cursorState = ExplorationCursorState.Selecting;
        }

        public void ReturnToMapView()
        {
            embarkHierarchy.SetActive(false);
            mapUIManager.DismissEmbarkUI();
            mapHierarchy.SetActive(true);
            AdventurerManager manager = ServiceLocator.Instance.GetService<AdventurerManager>();
            ResetCursor();
            manager.ResetStaging();
        }

        public void SetupPartyEmbark()
        {
            // Dismiss the map UI
            mapUIManager.DismissMapUI();

            // Clear any previous party embark data
            explorationTask.Reset();

            mapHierarchy.SetActive(false);
            
            // Set up the embark screen
            mapUIManager.ShowEmbarkUI();
            embarkHierarchy.SetActive(true);
        }

        public void EmbarkParty()
        {
            AdventurerManager manager = ServiceLocator.Instance.GetService<AdventurerManager>();
            Dictionary<int, Adventurer> stagingRoster = manager.GetStagingRoster();
            if (stagingRoster.Count == 0) {
                // Show some kind of message that at least one party member is required.
                mapUIManager.ShowError("You can't embark an empty party!");
                return;
            }
            // Make the exploration task
            ExplorationTask task = new ExplorationTask();
            task.SetCoordinates(_latestEvent.Coordinates);
            task.SetAssignedAdventurers(stagingRoster.Values.ToList());
            task.Duration = explorationMap.GetTraversalCost(_latestEvent.Coordinates);
            task.Title = $"Explore ({_latestEvent.Coordinates.Item1}, {_latestEvent.Coordinates.Item2})";
            task.ShortDescription = "A party has been sent to investigate what lies at an unknown place in the map";
            
            GameInstance.tasksToPopulate.Enqueue(task);
            ReturnToMapView();
        }

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