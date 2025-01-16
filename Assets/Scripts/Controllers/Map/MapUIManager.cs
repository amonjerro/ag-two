using UnityEngine;

namespace ExplorationMap
{
    public class MapUIManager : MonoBehaviour
    {



        public void HandleMapClickEvent(MapClickEvent clickEvent)
        {
            if (clickEvent.TileStatus == TileStatus.UNEXPLORED && clickEvent.IsExplorable) {
                ShowExplorationTaskUI(clickEvent);
            } else
            {
                ShowLocationDescriptionUI(clickEvent);
            }
        }

        private void ShowExplorationTaskUI(MapClickEvent clickEvent)
        {

        }

        private void ShowLocationDescriptionUI(MapClickEvent clickEvent)
        {

        }
    }
}
