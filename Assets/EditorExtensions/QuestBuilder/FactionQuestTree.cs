using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Linq;

namespace QuestBuilder
{
    public class FactionQuestTree : GraphView
    {
        public new class UxmlFactory : UxmlFactory<FactionQuestTree, UxmlTraits> { }

        FactionQuestTreeData rawDataTree;
        public Action<GraphTreeNode> OnNodeSelected;
        public bool NodeActivelySelected { get; private set; }
        private string nodeKey;
        FactionQuestEditor editorReference = null;

        public FactionQuestTree()
        {
            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/EditorExtensions/QuestBuilder/QuestBuilder.uss");
            styleSheets.Add(styleSheet);

            rawDataTree = new FactionQuestTreeData();
            InitializeTree();
            NodeActivelySelected = false;
        }

        public void ClearTree()
        {
            rawDataTree = new FactionQuestTreeData();
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;
        }

        public void InitializeTree()
        {
        }


        public void PopulateView(FactionQuestTreeData questData)
        {
            ClearTree();
            rawDataTree = questData;


            // Set up reference table
            Dictionary<string, FactionViewNode> nodeMap = new Dictionary<string, FactionViewNode>();
            List<FactionViewNode> viewNodes = new List<FactionViewNode>();

            // Create the nodes
            foreach (FactionQuestTreeNode questNode in rawDataTree.nodes)
            {
                FactionViewNode viewNode = InstantiateNodeElement(questNode);
                viewNode.SetPosition(new Rect(questNode.positionX, questNode.positionY, 0, 0));
                nodeMap.Add(questNode.questKey, viewNode);
                viewNodes.Add(viewNode);
            }


            // Create Edges
            foreach (FactionViewNode viewNode in viewNodes)
            {
                int childrenCount = viewNode.NodeData.children.Count;
                // If not the end node
                if (childrenCount > 0)
                {

                    // Create the edges
                    for (int i = 0; i < childrenCount; i++)
                    {
                        KeyPort key = viewNode.NodeData.children[i];

                        if (nodeMap.ContainsKey(key.childKey))
                        {
                            FactionViewNode child = nodeMap[key.childKey];
                            Edge e = viewNode.outputPorts[i].ConnectTo(child.inputPorts[key.portNumber]);
                            viewNode.AddChildConnection(child, i);
                            AddElement(e);
                        }
                    }
                }
            }
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            
            if (NodeActivelySelected)
            {
                BuildSelectedContextualMenu(evt);
            } else
            {
                BuildUnselectedContextualMenu(evt);
            }

        }

        private void BuildSelectedContextualMenu(ContextualMenuPopulateEvent evt)
        {
            
            evt.menu.AppendAction("Open", (a) => {
                QuestTreeEditor questEditor = (QuestTreeEditor)EditorWindow.GetWindow(typeof(QuestTreeEditor), false, null, true);
                questEditor.LoadByQuestId(nodeKey);
            });

        }

        private void BuildUnselectedContextualMenu(ContextualMenuPopulateEvent evt)
        {
            
            evt.menu.AppendSeparator();
            evt.menu.AppendAction("New Node", (a) => {
                CreateNode(a.eventInfo.localMousePosition.x, a.eventInfo.localMousePosition.y);
            });
        }

        public override List<Port> GetCompatiblePorts(Port start, NodeAdapter adapter)
        {
            return ports.ToList().Where(endPort =>
            endPort.direction != start.direction &&
            endPort.node != start.node).ToList();
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (editorReference != null)
            {
                editorReference.ChangesHaveOcurred();
            }

            // Delete elements in graph view
            if (graphViewChange.elementsToRemove != null)
            {
                foreach (GraphElement elem in graphViewChange.elementsToRemove)
                {
                    FactionViewNode nodeView = elem as FactionViewNode;
                    if (nodeView != null)
                    {
                        // Delete this node
                        rawDataTree.DeleteNode(nodeView.NodeData);

                    }

                    Edge edge = elem as Edge;
                    if (edge != null)
                    {
                        //Remove this edge
                        FactionViewNode parentView = edge.output.node as FactionViewNode;
                        FactionViewNode childView = edge.input.node as FactionViewNode;

                        int index = parentView.RemoveChild(childView);
                        rawDataTree.DisconnectNodes(parentView.NodeData, index);
                    }
                }
            }

            // Edges need to be created
            if (graphViewChange.edgesToCreate != null)
            {
                foreach (Edge edge in graphViewChange.edgesToCreate)
                {
                    FactionViewNode parentView = edge.output.node as FactionViewNode;
                    int outputPort = parentView.GetPortNumber(edge.output);
                    FactionViewNode childView = edge.input.node as FactionViewNode;
                    int inputPort = childView.GetPortNumber(edge.input, false);

                    parentView.AddChildConnection(childView, outputPort);
                    childView.Parent = parentView;
                    rawDataTree.ConnectNodes(parentView.NodeData, childView.NodeData, outputPort, inputPort);
                }
            }

            return graphViewChange;

        }

        private FactionViewNode CreateNode(float positionX, float positionY)
        {
            if (editorReference != null)
            {
                editorReference.ChangesHaveOcurred();
            }

            FactionQuestTreeNode nodeData = new FactionQuestTreeNode();
            nodeData.questKey = GUID.Generate().ToString();
            rawDataTree.nodes.Add(nodeData);
            FactionViewNode fvn = InstantiateNodeElement(nodeData);
            fvn.SetPosition(new Rect(positionX, positionY, 0,0));
            return fvn;
        }

        private FactionViewNode InstantiateNodeElement(FactionQuestTreeNode data)
        {
            FactionViewNode viewNode = new FactionViewNode(data);
            viewNode.OnNodeSelected = HandleNodeSelect;
            viewNode.OnNodeUnselected = HandleNodeUnselect;
            AddElement(viewNode);
            return viewNode;
        }

        private void HandleNodeSelect(GraphTreeNode node)
        {
            NodeActivelySelected = true;
            nodeKey = node.GetKey();
            OnNodeSelected?.Invoke(node);
        }

        private void HandleNodeUnselect()
        {
            NodeActivelySelected = false;
        }

        public FactionQuestTreeData GetNodeTree()
        {
            return rawDataTree;
        }

        public void SetEditorReference(FactionQuestEditor qte)
        {
            editorReference = qte;
        }


    }
}
