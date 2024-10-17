using System.Collections.Generic;
using UnityEngine;

public partial class QuestController
{
    private static class QuestNodeFactory
    {
        public static AQuestNode MakeQuestNode(NodeTypes type)
        {
            switch (type) {
                case NodeTypes.Start:
                case NodeTypes.Decision:
                    return new DecisionNode();
                default:
                    return new InformationNode();
            }
        }
    }

    private static class QuestLoader
    {

        private static NodeTypes MapStringToNodeType(string key)
        {
            switch (key) {
                default:
                    return NodeTypes.Info;
            }
        }

        public static void LoadQuest(SO_QuestData soQuest, QuestData dataQuest)
        {
            Dictionary<string, AQuestNode> nodeObjectMap = new Dictionary<string, AQuestNode>();
            Dictionary<string, QuestNodeData> dataObjectMap = new Dictionary<string, QuestNodeData>();
            QuestNodeArray questNodeData = JsonUtility.FromJson<QuestNodeArray>(soQuest.nodes.text);
            QuestNodeData[] nodes = questNodeData.nodes;

            // Get the root node of this quest
            string rootKey = nodes[0].key;

            DecisionNode startNode = (DecisionNode)QuestNodeFactory.MakeQuestNode(NodeTypes.Start);
            string[] startStrings = {Constants.QuestChooseAdventurers, Constants.QuestClose};
            startNode.SetButtonStrings(startStrings);
            startNode.Description = dataQuest.description;
            startNode.Title = dataQuest.questTitle;
            startNode.NodeType = NodeTypes.Start;
            startNode.QuestKey = dataQuest.questKey;

            // Set up the nodes objects and index their keys
            for (int i = 0; i < nodes.Length; i++)
            {
                NodeTypes type = MapStringToNodeType(nodes[i].type);

                AQuestNode node = QuestNodeFactory.MakeQuestNode(type);

                node.Duration = nodes[i].duration;
                node.Description = nodes[i].description;
                node.Title = nodes[i].title;
                node.CurrentTickCount = 0;
                node.SetButtonStrings(nodes[i].buttonStrings);
                node.NodeType = type;
                node.QuestKey = dataQuest.questKey;
                nodeObjectMap.Add(nodes[i].key, node);
                dataObjectMap.Add(nodes[i].key, nodes[i]);
            }

            // By key, bind the objects in the object map
            foreach(KeyValuePair<string, AQuestNode> pair in nodeObjectMap)
            {
                string nextKey = dataObjectMap[pair.Key].next;
                if (nextKey == "")
                {
                    pair.Value.Next = null;
                } else
                {
                    pair.Value.Next = nodeObjectMap[nextKey];
                }
            }

            startNode.Next = nodeObjectMap[rootKey];

            // Set the root node of the quest tree
            dataQuest.rootNode = startNode;
        }
    }
}