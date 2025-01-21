using System.Collections.Generic;

namespace ExplorationMap
{
    public class EmbarkInterface : InterfaceCollection
    {
        StatBlock[] statBlocks;
        bool bBound = false;

        private void Start()
        {

            BindStatBlocks();
            bBound = true;
            
        }

        private void BindStatBlocks()
        {
            statBlocks = transform.GetComponentsInChildren<StatBlock>();
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
    }
}