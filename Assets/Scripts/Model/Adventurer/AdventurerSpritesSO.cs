using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = Constants.ScriptableObjectsPath+"AdventurerSpriteset")]
public class AdventurerSpritesSO : ScriptableObject
{
    public List<Sprite> heads;
    public List<Sprite> bodies;
    public List<Sprite> torsos;
    public List<Sprite> legs;
}