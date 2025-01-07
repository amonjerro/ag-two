using System.Collections.Generic;
using UnityEngine;

namespace ExplorationMap
{
    [CreateAssetMenu(menuName = Constants.ScriptableObjectsPath + "TileSprite")]
    public class MapTileSpritesSO : ScriptableObject
    {
        [System.Serializable]
        public struct OrientationSprite
        {
            public ConnectionOrientations connectionOrientation;
            public Sprite sprite;
        }

        [SerializeField]
        List<OrientationSprite> roadSprites = new List<OrientationSprite>();
        [SerializeField]
        List<OrientationSprite> forestSprites = new List<OrientationSprite>();
        [SerializeField]
        List<OrientationSprite> mountainSprites = new List<OrientationSprite>();
        [SerializeField]
        List<OrientationSprite> lakeSprites = new List<OrientationSprite>();

        public Dictionary<ConnectionOrientations, Sprite> roadSpriteDict = new Dictionary<ConnectionOrientations, Sprite>();
        public Dictionary<ConnectionOrientations, Sprite> forestSpriteDict = new Dictionary<ConnectionOrientations, Sprite>();
        public Dictionary<ConnectionOrientations, Sprite> mountainSpriteDict = new Dictionary<ConnectionOrientations, Sprite>();
        public Dictionary<ConnectionOrientations, Sprite> lakeSpriteDict = new Dictionary<ConnectionOrientations, Sprite>();

        public void SetUpData()
        {
            foreach(OrientationSprite os in roadSprites)
            {
                roadSpriteDict.Add(os.connectionOrientation, os.sprite);
            }

            foreach (OrientationSprite os in forestSprites)
            {
                forestSpriteDict.Add(os.connectionOrientation, os.sprite);
            }

            foreach (OrientationSprite os in mountainSprites)
            {
                mountainSpriteDict.Add(os.connectionOrientation, os.sprite);
            }

            foreach (OrientationSprite os in lakeSprites)
            {
                lakeSpriteDict.Add(os.connectionOrientation, os.sprite);
            }
        }

    }
}
