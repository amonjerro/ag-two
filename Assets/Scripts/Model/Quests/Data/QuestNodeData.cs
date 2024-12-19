using JetBrains.Annotations;
using System.Collections.Generic;

[System.Serializable]
public class ChallengeValues
{
    public int nature;
    public int combat;
    public int dungeoneering;
    public int social;
    public int arcane;

    public Stats ToStats()
    {
        Stats stats = new Stats(combat, dungeoneering, nature, social, arcane);
        return stats;
    }
}


[System.Serializable]
public class QuestNodeData
{
    public string key;
    public string type;
    public string title;
    public string description;
    public string[] buttonStrings;
    public bool providesReward;
    public int duration;
    public List<string> next;
    public ChallengeValues challengeValues;

    public float positionX;
    public float positionY;

    public QuestNodeData()
    {
        next = new List<string>();
    }

    public void CreateNextSlots(int slots)
    {
        for (int i = 0; i < slots; i++) {
            next.Add("");
        }
    }
}

[System.Serializable]
public class QuestNodeArray
{
    public List<QuestNodeData> nodes;

    public QuestNodeArray()
    {
        nodes = new List<QuestNodeData>();
    }

    public void DeleteNode(QuestNodeData node)
    {
        // Disconnect the item
        foreach (QuestNodeData nodeData in nodes) {
            if (nodeData.next.Contains(node.key))
            {
                int indexOf = nodeData.next.IndexOf(node.key);
                nodeData.next[indexOf] = "";
                break;
            }
        }

        // Remove the item
        nodes.Remove(node);
    }

    public void DisconnectNodes(QuestNodeData parent, int childIndex)
    {
        parent.next[childIndex] = "";
    }

    public void ConnectNodes(QuestNodeData parent, QuestNodeData child, int edgeNumber)
    {
        parent.next[edgeNumber] = child.key;
    }
}

