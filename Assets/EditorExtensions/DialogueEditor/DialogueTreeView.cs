using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Linq;

namespace DialogueEditor
{
    public class DialogueTreeView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<DialogueTreeView, UxmlTraits> { }

        DialogueNodeTree rawDataTree;
        public Action<DialogueViewNode> OnNodeSelected;
        DialogueEditorWindow editorReference = null;

        public DialogueTreeView()
        {
            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/EditorExtensions/QuestBuilder/QuestBuilder.uss");
            styleSheets.Add(styleSheet);

            rawDataTree = new DialogueNodeTree();
            InitializeTree();
        }

        public void ClearTree()
        {
            rawDataTree = new DialogueNodeTree();
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;
        }

        public void InitializeTree()
        {
            // Create a start and end node
            graphViewChanged -= OnGraphViewChanged;
            DialogueViewNode startNode = CreateNode(NodeTypes.Start);
            startNode.title = "Start Node";
            startNode.nodeData.name = "sn1";
            graphViewChanged += OnGraphViewChanged;
        }


        public void PopulateView(DialogueNodeTree questData)
        {
            ClearTree();
            rawDataTree = questData;


            // Set up reference table
            Dictionary<string, DialogueViewNode> nodeMap = new Dictionary<string, DialogueViewNode>();
            List<DialogueViewNode> viewNodes = new List<DialogueViewNode>();

            // Create the nodes
            foreach (DialogueNodeData nodeData in rawDataTree.passages)
            {
                DialogueViewNode viewNode = InstantiateNodeElement(nodeData);
                viewNode.SetPosition(new Rect(nodeData.position.x, nodeData.position.y, 0, 0));
                nodeMap.Add(nodeData.name, viewNode);
                viewNodes.Add(viewNode);
            }


            // Create Edges
            foreach (DialogueViewNode viewNode in viewNodes)
            {

                // If not the end node
                if (viewNode.nodeData.links.Count > 0)
                {

                    // Create the edges
                    for (int i = 0; i < viewNode.nodeData.links.Count; i++)
                    {
                        Link link = viewNode.nodeData.links[i];

                        if (nodeMap.ContainsKey(link.link))
                        {
                            DialogueViewNode child = nodeMap[link.link];
                            child.CreateInputNode();
                            viewNode.CreateNewOutputPort();
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
            foreach (NodeTypes nodeType in Enum.GetValues(typeof(NodeTypes)))
            {
                string name = nodeType.ToString();
                evt.menu.AppendAction(name + " Node", (a) => {
                    DialogueViewNode node = CreateNode(nodeType);
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
                    DialogueViewNode nodeView = elem as DialogueViewNode;
                    if (nodeView != null)
                    {
                        // Delete this node
                        rawDataTree.DeleteNode(nodeView.nodeData);

                    }

                    Edge edge = elem as Edge;
                    if (edge != null)
                    {
                        //Remove this edge
                        DialogueViewNode parentView = edge.output.node as DialogueViewNode;
                        DialogueViewNode childView = edge.input.node as DialogueViewNode;

                        int index = parentView.RemoveChild(childView);
                        rawDataTree.DisconnectNodes(parentView.nodeData, index);
                    }
                }
            }

            // Edges need to be created
            if (graphViewChange.edgesToCreate != null)
            {
                foreach (Edge edge in graphViewChange.edgesToCreate)
                {
                    DialogueViewNode parentView = edge.output.node as DialogueViewNode;
                    int outputPort = parentView.GetPortNumber(edge.output);
                    DialogueViewNode childView = edge.input.node as DialogueViewNode;
                    parentView.AddChildConnection(childView, outputPort);
                    childView.Parent = parentView;
                    rawDataTree.ConnectNodes(parentView.nodeData, childView.nodeData, outputPort);
                }
            }

            return graphViewChange;

        }

        private DialogueViewNode CreateNode(NodeTypes type)
        {
            if (editorReference != null)
            {
                editorReference.ChangesHaveOcurred();
            }
            DialogueNodeData nodeData = new DialogueNodeData();
            nodeData.pid = GUID.Generate().ToString();
            rawDataTree.passages.Add(nodeData);
            DialogueViewNode dvn = InstantiateNodeElement(nodeData);
            return dvn;
        }

        private DialogueViewNode InstantiateNodeElement(DialogueNodeData data)
        {
            DialogueViewNode viewNode = new DialogueViewNode(data);
            viewNode.OnNodeSelected = OnNodeSelected;
            AddElement(viewNode);
            return viewNode;
        }

        public DialogueNodeTree GetNodeTree()
        {
            return rawDataTree;
        }

        public void SetEditorReference(DialogueEditorWindow qte)
        {
            editorReference = qte;
        }
    }
}
