using Unity.VisualScripting;

[System.Serializable]
public class QuestNodeData
{
    public string key;
    public string type;
    public string title;
    public string description;
    public string[] buttonStrings;
    public int duration;
    public string next;

    public string guid;
    public float positionX;
    public float positionY;
}

[System.Serializable]
public class QuestNodeArray
{
    public QuestNodeData[] nodes;
}