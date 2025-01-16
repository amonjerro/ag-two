using UnityEngine;

namespace ExplorationMap
{
    public class MapUIManager : MonoBehaviour
    {
        [SerializeField]
        UIPanel ExplorationTaskPanel;
        
        [SerializeField]
        UIPanel LocationDescriptionPanel;

        public void HandleMapClickEvent(MapClickEvent clickEvent)
        {
            if (clickEvent.TileStatus == TileStatus.UNEXPLORED && clickEvent.IsExplorable) {
                ShowExplorationTaskUI(clickEvent);
            } else if (clickEvent.TileStatus == TileStatus.EXPLORED)
            {
                ShowLocationDescriptionUI(clickEvent);
            } else
            {
                DismissMapUI();
            }
        }

        private void ShowExplorationTaskUI(MapClickEvent clickEvent)
        {
            LocationDescriptionPanel.Dismiss();
            ExplorationTaskPanel.Show();
        }

        private void ShowLocationDescriptionUI(MapClickEvent clickEvent)
        {
            ExplorationTaskPanel.Dismiss();
            LocationDescriptionPanel.Show();
        }

        public void DismissMapUI()
        {
            LocationDescriptionPanel.Dismiss();
            ExplorationTaskPanel.Dismiss();
        }
    }
}
