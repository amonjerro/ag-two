using UnityEngine;

[CreateAssetMenu(menuName = Constants.ScriptableObjectsPath+"Faction")]
public class FactionDetails : ScriptableObject
{
    public string factionName;
    public Sprite factionSprite;
    public Sprite factionUISprite;
    public Faction faction;
}