using System.Collections.Generic;
using UnityEngine;

namespace Rooms
{
    [CreateAssetMenu(menuName = Constants.ScriptableObjectsPath + "RoomData")]
    public class RoomData : ScriptableObject
    {
        public string roomName;
        public Sprite roomSprite;
        public RoomType roomType;
        public List<ComponentData> componentData;
    }
}