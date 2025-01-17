using System.Collections.Generic;

namespace ExplorationMap
{
    public class EmbarkInterface : InterfaceCollection
    {
        StatBlock[] statBlocks;

        private void Start()
        {

            BindStatBlocks();
        }

        private void BindStatBlocks()
        {
            statBlocks = transform.GetComponentsInChildren<StatBlock>();
        }

        public void UpdateStatBlocks(Adventurer adventurer)
        {
            
        }
    }
}