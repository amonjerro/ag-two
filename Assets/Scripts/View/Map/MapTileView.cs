using System.Collections.Generic;
using UnityEngine;

namespace ExplorationMap { 
    public class MapTileView : MonoBehaviour
    {
        ExplorationMapManager mapManagerRef;
        ExplorationMap mapReference;
        SpriteRenderer spriteRenderer;

        private int locationX, locationY;

        private void Start()
        {
            mapManagerRef = ServiceLocator.Instance.GetService<ExplorationMapManager>();
            mapReference = mapManagerRef.GetMapReference();
            spriteRenderer = GetComponent<SpriteRenderer>();
            ExplorationMap.tileRevealEvent += UpdateLooks;
            spriteRenderer.color = Color.black;
            locationX = Mathf.FloorToInt(transform.position.x);
            locationY = Mathf.FloorToInt(transform.position.y);

            if (mapReference.GetTileStatus((locationX, locationY)) == TileStatus.EXPLORED)
            {
                UpdateLooks((locationX, locationY));
            }
        }

        private void OnDestroy()
        {
            ExplorationMap.tileRevealEvent -= UpdateLooks;
        }

        private void UpdateLooks((int, int) coordinates)
        {
            if (coordinates.Item1 == locationX && coordinates.Item2 == locationY) {
                List<Sprite> sprites = mapManagerRef.GetSpritesForMapTile(coordinates);
                Sprite result = SpriteMerger.MergeSprites(sprites);
                spriteRenderer.color = Color.white;
                spriteRenderer.sprite = result;
            }
        }
    }
}