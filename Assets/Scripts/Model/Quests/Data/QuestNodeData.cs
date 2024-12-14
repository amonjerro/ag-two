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
    public string next;

    public string guid;
    public float positionX;
    public float positionY;
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
            if (nodeData.next == node.key) {
                nodeData.next = "";
                break;
            }
        }

        // Remove the item
        nodes.Remove(node);
    }

    public void ConnectNodes(QuestNodeData parent, QuestNodeData child, Edge edge)
    {
        parent.next = child.key;
    }
}

