using UnityEngine;

namespace ExplorationMap
{
    // Controls the UI of the Operations Scene. All of it.
    public class MapUIManager : MonoBehaviour
    {
        [SerializeField]
        UIPanel ExplorationTaskPanel;
        
        [SerializeField]
        UIPanel LocationDescriptionPanel;

        [SerializeField]
        EmbarkInterface embarkInterface;

        [SerializeField]
        RosterWidget rosterWidget;

        [SerializeField]
        ErrorNotification errorNotification;

        public bool WillShowGUI(MapClickEvent clickEvent)  {
            return clickEvent.TileStatus == TileStatus.EXPLORED || (clickEvent.TileStatus == TileStatus.UNEXPLORED && clickEvent.IsExplorable);
        }

        public void HandleMapClickEvent(MapClickEvent clickEvent)
        {
            if (clickEvent.TileStatus == TileStatus.EXPLORED) {
                ShowLocationDescriptionUI(clickEvent);
            } else if (clickEvent.TileStatus == TileStatus.UNEXPLORED && clickEvent.IsExplorable)
            {
                ShowExplorationTaskUI(clickEvent);
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

        public void ShowEmbarkUI()
        {
            embarkInterface.SetActive(true);
            embarkInterface.UpdateStatBlocks();
            embarkInterface.UpdateButtons();
        }

        public void DismissEmbarkUI()
        {
            embarkInterface.SetActive(false);
        }

        public void DismissMapUI()
        {
            LocationDescriptionPanel.Dismiss();
            ExplorationTaskPanel.Dismiss();
        }

        public void ShowRosterUI(int i)
        {
            rosterWidget.gameObject.SetActive(true);
            rosterWidget.SetRosterIndex(i);
            rosterWidget.Show();
        }

        public void DismissRosterUI()
        {
            rosterWidget.Dismiss();
        }

        public void ShowError(string text)
        {
            errorNotification.SetMessage(text);
            errorNotification.Show();
        }
    }
}
