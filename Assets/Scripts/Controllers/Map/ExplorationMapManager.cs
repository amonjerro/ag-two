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

        private int currentX;
        private int currentY;

        private void Awake()
        {
            explorationMap = new ExplorationMap(mapTileData);
            currentX = 0;
            currentY = 0;
        }
        private void Start()
        {
            
        }

        private void Update()
        {
            
        }

        private void OnMove(InputValue value)
        {

        }

        private void OnSelect(InputValue value) { 
        }

        public ExplorationMap GetMapReference() { return explorationMap; }
    }
}