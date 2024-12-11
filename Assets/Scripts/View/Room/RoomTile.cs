using UnityEngine;

namespace Rooms
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class RoomTile : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public RoomType roomType;
        public int posX;
        public int posY;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = ServiceLocator.Instance.GetService<RoomManager>().GetSpriteByType(roomType);
        }
    }
}