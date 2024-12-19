using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Linq;

namespace QuestBuilder {
    public class NodeTreeView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<NodeTreeView, UxmlTraits> { }

        QuestNodeArray rawDataTree;
        public Action<QuestViewNode> OnNodeSelected;
        QuestTreeEditor editorReference = null;

        public NodeTreeView()
        {
            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/EditorExtensions/QuestBuilder/QuestBuilder.uss");
            styleSheets.Add(styleSheet);

            rawDataTree = new QuestNodeArray();
            InitializeTree();
        }

        public void ClearTree()
        {
            rawDataTree = new QuestNodeArray();
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;
        }

        public void InitializeTree()
        {
            // Create a start and end node
            graphViewChanged -= OnGraphViewChanged;
            QuestViewNode startNode = CreateNode(NodeTypes.Start);
            startNode.title = "Start Node";
            startNode.questNode.title = "Start Node";
            QuestViewNode endNode = CreateNode(NodeTypes.End);
            endNode.title = "Decline Quest";
            endNode.questNode.title = "Decline Quest";

            endNode.SetPosition(new Rect(200, 200, 0, 0));
            Edge e = startNode.outputPorts[1].ConnectTo(endNode.input);
            startNode.AddChildConnection(endNode, 1);
            rawDataTree.ConnectNodes(startNode.questNode, endNode.questNode, 1);

            AddElement(e);
            graphViewChanged += OnGraphViewChanged;
        }


        public void PopulateView(QuestNodeArray questData)
        {
            ClearTree();
            rawDataTree = questData;
            

            // Set up reference table
            Dictionary<string, QuestViewNode> nodeMap = new Dictionary<string, QuestViewNode>();
            List<QuestViewNode> viewNodes = new List<QuestViewNode>();
            
            // Create the nodes
            foreach(QuestNodeData questNode in rawDataTree.nodes)
            {
                QuestViewNode viewNode = InstantiateNodeElement(questNode);
                viewNode.SetPosition(new Rect(questNode.positionX, questNode.positionY, 0, 0));
                nodeMap.Add(questNode.key, viewNode);
                viewNodes.Add(viewNode);
            }


            // Create Edges
            foreach (QuestViewNode viewNode in viewNodes) {

                // If not the end node
                if (viewNode.questNode.next.Count > 0) {

                    // Create the edges
                    for (int i = 0; i < viewNode.questNode.next.Count; i++) {
                        string key = viewNode.questNode.next[i];
                        
                        if (nodeMap.ContainsKey(key)) {
                            QuestViewNode child = nodeMap[key];
                            Edge e = viewNode.outputPorts[i].ConnectTo(child.input);
                            viewNode.AddChildConnection(child, i);
                            AddElement(e);
                        }
                    }
                }
            }
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);

            evt.menu.AppendSeparator();
            foreach(NodeTypes nodeType in Enum.GetValues(typeof(NodeTypes)))
            {
                string name = nodeType.ToString();
                evt.menu.AppendAction(name + " Node", (a) => {
                    QuestViewNode node = CreateNode(nodeType);
                    node.SetPosition(new Rect(a.eventInfo.mousePosition, Vector2.zero));
                });
            }
        }

        public override List<Port> GetCompatiblePorts(Port start, NodeAdapter adapter)
        {
            return ports.ToList().Where(endPort => 
            endPort.direction != start.direction &&
            endPort.node != start.node).ToList();
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange) {
            if (editorReference != null)
            {
                editorReference.ChangesHaveOcurred();
            }
            
            // Delete elements in graph view
            if(graphViewChange.elementsToRemove != null)
            {
                foreach(GraphElement elem in graphViewChange.elementsToRemove)
                {
                    QuestViewNode nodeView = elem as QuestViewNode;
                    if(nodeView != null)
                    {
                        // Delete this node
                        rawDataTree.DeleteNode(nodeView.questNode);

                    }

                    Edge edge = elem as Edge;
                    if (edge != null)
                    {
                        //Remove this edge
                        QuestViewNode parentView = edge.output.node as QuestViewNode;
                        QuestViewNode childView = edge.input.node as QuestViewNode;

                        int index = parentView.RemoveChild(childView);
                        rawDataTree.DisconnectNodes(parentView.questNode, index);
                    }
                }
            }

            // Edges need to be created
            if(graphViewChange.edgesToCreate != null)
            {
                foreach(Edge edge in graphViewChange.edgesToCreate)
                {
                    QuestViewNode parentView = edge.output.node as QuestViewNode;
                    int outputPort = parentView.GetPortNumber(edge.output);
                    QuestViewNode childView = edge.input.node as QuestViewNode;
                    parentView.AddChildConnection(childView, outputPort);
                    childView.Parent = parentView;
                    rawDataTree.ConnectNodes(parentView.questNode, childView.questNode, outputPort);
                }
            }

            return graphViewChange;

        }

        private QuestViewNode CreateNode(NodeTypes type)
        {
            if (editorReference != null)
            {
                editorReference.ChangesHaveOcurred();
            }
            QuestNodeData nodeData = new QuestNodeData();
            switch (type)
            {
                case NodeTypes.Challenge:
                    nodeData.buttonStrings = new string[2];
                    nodeData.CreateNextSlots(2);
                    nodeData.challengeValues = new ChallengeValues();
                    break;
                case NodeTypes.Decision:
                case NodeTypes.Start:
                    nodeData.buttonStrings = new string[2];
                    nodeData.CreateNextSlots(2);
                    break;
                case NodeTypes.End:
                    nodeData.buttonStrings = new string[1];
                    break;
                default:
                    nodeData.buttonStrings = new string[1];
                    nodeData.CreateNextSlots(1);
                    break;
            }
            nodeData.type = QuestController.MapTypeToString(type);
            nodeData.key = GUID.Generate().ToString();
            rawDataTree.nodes.Add(nodeData);
            QuestViewNode qvn = InstantiateNodeElement(nodeData);
            return qvn;
        }

        private QuestViewNode InstantiateNodeElement(QuestNodeData data)
        {
            QuestViewNode viewNode = new QuestViewNode(data);
            viewNode.OnNodeSelected = OnNodeSelected;
            AddElement(viewNode);
            return viewNode;
        }

        public QuestNodeArray GetNodeTree()
        {
            return rawDataTree;
        }

        public void SetEditorReference(QuestTreeEditor qte)
        {
            editorReference = qte;
        }
    }
}
