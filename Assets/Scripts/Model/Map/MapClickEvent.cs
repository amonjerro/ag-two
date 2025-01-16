namespace ExplorationMap
{
    public class MapClickEvent
    {
        public TileStatus TileStatus {  get; set; }
        public (int, int) Coordinates { get; set; }

        public bool IsExplorable { get; set; }

    }
}