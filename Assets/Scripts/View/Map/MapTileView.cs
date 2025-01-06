using UnityEngine;

namespace ExplorationMap { 
    public class MapTileView : MonoBehaviour
    {
        ExplorationMap mapReference;

        private void Start()
        {
            mapReference = ServiceLocator.Instance.GetService<ExplorationMapManager>().GetMapReference();
            ExplorationMap.tileRevealEvent += UpdateLooks;
        }

        private void OnDestroy()
        {
            ExplorationMap.tileRevealEvent -= UpdateLooks;
        }

        private void UpdateLooks((int, int) coordinates)
        {
            
        }
    }
}