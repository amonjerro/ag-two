using System;
using System.Collections.Generic;

namespace DialogueEditor
{
    [Serializable]
    public class Position
    {
        public float x;
        public float y;
    }

    [Serializable]
    public class Link
    {
        public string name;
        public string link;
        public string pid;
    }

    [Serializable]
    public class DialogueNodeData
    {
        public string name;
        public string text;
        public string pid;
        public Position position;
        public List<Link> links;

        public DialogueNodeData()
        {
            links = new List<Link>();
            position = new Position();
        }

        public void CreateNextSlots(int slots)
        {
            for (int i = 0; i < slots; i++)
            {
                links.Add(new Link());
            }
        }

        public void UpdateLinkText(int i, string text)
        {
            links[i].name = text;
        }

        public int LinksContains(string name)
        {
            for (int i = 0; i < links.Count; i++)
            {
                if (links[i].link == name) return i;
            }
            return -1;
        }

        public void ClearLink(int i)
        {
            links[i] = new Link();
        }
    }

    public enum AffinityInfluenceTypes
    {

    }

    [Serializable]
    public class DialogueNodeTree
    {
        public List<DialogueNodeData> passages;
        public string name;
        public string ifid;
        public string creator;
        public string creatorVersion;
        public string startNode;

        public DialogueNodeTree()
        {
            passages = new List<DialogueNodeData>();
        }

        public void DeleteNode(DialogueNodeData node)
        {
            // Disconnect the item
            foreach (DialogueNodeData nodeData in passages)
            {
                int linkIndex = nodeData.LinksContains(node.name);
                if (linkIndex >= 0)
                {
                    nodeData.ClearLink(linkIndex);
                    break;
                }
            }

            // Remove the item
            passages.Remove(node);
        }

        public void DisconnectNodes(DialogueNodeData parent, int childIndex)
        {
            parent.ClearLink(childIndex);
        }

        public void ConnectNodes(DialogueNodeData parent, DialogueNodeData child, int edgeNumber)
        {
            parent.links[edgeNumber].link = child.name;
        }
    }
}
