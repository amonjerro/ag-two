using DialogueEditor;
using JetBrains.Annotations;
using System.Collections.Generic;

[System.Serializable]
public enum RequirementType
{
    AND,
    OR
}

[System.Serializable]
public enum RetryOptions
{
    None,
    Unlimited
}

[System.Serializable]
public class KeyPort
{
    public string childKey;
    public int portNumber;

    public KeyPort()
    {
        childKey = "";
        portNumber = 0;
    }

    public KeyPort(string childKey, int portNumber)
    {
        this.childKey = childKey;
        this.portNumber = portNumber;
    }
}

[System.Serializable]
public class FactionQuestTreeNode
{
    public string questKey;
    public Faction faction;
    public string name;
    public string description;
    public RequirementType requirementType;
    public List<KeyPort> children;
    public RetryOptions retryOptions;
    public int minimumTicks;

    public float positionX;
    public float positionY;

    public void AddChildrenSlot(int slots){
        for(int i = 0; i < slots; i++)
        {
            children.Add(null);
        }
    }

    public int ChildKeyIndex(string key)
    {
        for (int i = 0; i < children.Count; i++)
        {
            if (children[i].childKey == key) return i;
        }
        return -1;
    }
}

[System.Serializable]
public class FactionQuestTreeData
{
    public List<FactionQuestTreeNode> nodes;

    public FactionQuestTreeData()
    {
        nodes = new List<FactionQuestTreeNode>();
    }

    public void DeleteNode(FactionQuestTreeNode node)
    {
        foreach (FactionQuestTreeNode child in nodes) {
            int childKey = child.ChildKeyIndex(node.questKey);
            if (childKey > -1) {
                child.children[childKey] = null;
            }
        }
        nodes.Remove(node);
    }

    public void ConnectNodes(FactionQuestTreeNode parent, FactionQuestTreeNode child, int childIndex, int inputPort)
    {
        parent.children[childIndex] = new KeyPort(child.questKey, inputPort);
    }

    public void DisconnectNodes(FactionQuestTreeNode parent, int childIndex) { 
        parent.children[childIndex] = null;
    }

}