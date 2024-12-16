using System.Collections.Generic;
#if (UNITY_EDITOR)
using UnityEditor.Experimental.GraphView;
#endif

[System.Serializable]
public class QuestNodeData
{
    public string key;
    public string type;
    public string title;
    public string description;
    public string[] buttonStrings;
    public int duration;
    public List<string> next;

    public float positionX;
    public float positionY;

    public QuestNodeData()
    {
        next = new List<string>();
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

    public void ConnectNodes(QuestNodeData parent, QuestNodeData child, Edge edge)
    {
        parent.next.Add(child.key);
    }
}

