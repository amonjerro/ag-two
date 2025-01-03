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
public class FactionQuestTreeNode
{
    public string questKey;
    public Faction faction;
    public string name;
    public string description;
    public RequirementType requirementType;
    public List<string> children;
    public RetryOptions retryOptions;
    public int minimumTicks;

    public void AddChildrenSlot(int slots){
        for(int i = 0; i < slots; i++)
        {
            children.Add("");
        }
    }
}

[System.Serializable]
public class FactionQuestTreeData
{
    public List<FactionQuestTreeNode> nodes;

    public void DeleteNode(FactionQuestTreeNode node)
    {
        foreach (FactionQuestTreeNode child in nodes) { 
            if (child.children.Contains(node.questKey))
            {
                int indexOf = child.children.IndexOf(node.questKey);
                child.children[indexOf] = "";
            }
        }
        nodes.Remove(node);
    }

    public void ConnectNodes(FactionQuestTreeNode parent, FactionQuestTreeNode child, int childIndex)
    {
        parent.children[childIndex] = child.questKey;
    }

    public void DisconnectNodes(FactionQuestTreeNode parent, int childIndex) { 
        parent.children[childIndex] = "";
    }

}