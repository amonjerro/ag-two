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
            RoomManager roomManagerRef = ServiceLocator.Instance.GetService<RoomManager>();
            spriteRenderer.sprite = roomManagerRef.GetSpriteByType(roomType);
            roomManagerRef.RegisterTile((posX, posY), this);

        }

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }

        public void SetType(RoomType type)
        {
            roomType = type;
        }
    }
}