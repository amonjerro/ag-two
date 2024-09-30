
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = Constants.ScriptableObjectsPath+"Quests")]
public class SO_QuestData : ScriptableObject
{
    public Faction faction;
    public string questKey;
    public string questTitle;
    public string description;
    public int startTick;

    public TextAsset nodes;
}