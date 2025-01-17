namespace ExplorationMap
{
    // Bundles information to communicate where the player has clicked
    public struct MapClickEvent
    {
        public TileStatus TileStatus {  get; set; }
        public (int, int) Coordinates { get; set; }

        public bool IsExplorable { get; set; }

    }
}