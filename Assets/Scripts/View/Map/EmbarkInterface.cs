using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

namespace ExplorationMap
{
    public class EmbarkInterface : InterfaceCollection
    {
        StatBlock[] statBlocks;
        Button[] embarkButtons;
        bool bBound = false;

        private void Start()
        {

            BindWidgets();
            bBound = true;
            
        }

        private void BindWidgets()
        {
            statBlocks = transform.GetComponentsInChildren<StatBlock>();
            EmbarkPanel embarkPanel = transform.GetComponentInChildren<EmbarkPanel>();
            embarkButtons = embarkPanel.transform.GetComponentsInChildren<Button>();
        }

        public void UpdateStatBlocks()
        {

            if (!bBound) {
                return;
            }
            AdventurerManager manager = ServiceLocator.Instance.GetService<AdventurerManager>();
            Stats partyStats = new Stats(0, 0, 0, 0, 0);
            foreach (KeyValuePair<int, Adventurer> kvp in manager.GetStagingRoster())
            {
                 partyStats.Add(kvp.Value.Char_Stats);
            }

            foreach (StatBlock sb in statBlocks) { 
                sb.UpdateStatSliderValue(partyStats);
            }
        }

        public void UpdateButtons()
        {
            AdventurerManager manager = ServiceLocator.Instance.GetService<AdventurerManager>();
            Dictionary<int, Adventurer> stagingRoster = manager.GetStagingRoster();
            for (int i = 0; i < embarkButtons.Length; i++)
            {
                if (stagingRoster.ContainsKey(i))
                {
                    embarkButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = stagingRoster[i].Name;
                } else
                {
                    embarkButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Pick";
                    if (manager.AvailabilityCount() == 0)
                    {
                        embarkButtons[i].interactable = false;
                    }
                }
            }
        }
    }
}