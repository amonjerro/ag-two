using System.Collections.Generic;
using UnityEngine;

namespace ExplorationMap { 
    public class MapTileView : MonoBehaviour
    {
        ExplorationMapManager mapManagerRef;
        ExplorationMap mapReference;
        SpriteRenderer spriteRenderer;

        private void Start()
        {
            mapManagerRef = ServiceLocator.Instance.GetService<ExplorationMapManager>();
            mapReference = mapManagerRef.GetMapReference();
            spriteRenderer = GetComponent<SpriteRenderer>();
            ExplorationMap.tileRevealEvent += UpdateLooks;
        }

        private void OnDestroy()
        {
            ExplorationMap.tileRevealEvent -= UpdateLooks;
        }

        private void UpdateLooks((int, int) coordinates)
        {
            List<Sprite> sprites = mapManagerRef.GetSpritesForMapTile(coordinates);
            Sprite result = SpriteMerger.MergeSprites(sprites);
            spriteRenderer.sprite = result;
        }
    }
}