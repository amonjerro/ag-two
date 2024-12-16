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
            graphViewChanged += OnGraphViewChanged;
        }

        public void PopulateView(QuestNodeArray questData)
        {
            rawDataTree = questData;
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;


            
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);

            evt.menu.AppendSeparator();
            foreach(NodeTypes nodeType in Enum.GetValues(typeof(NodeTypes)))
            {
                string name = nodeType.ToString();
                evt.menu.AppendAction(name + " Node", (a) => CreateNode(nodeType));
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
                    }
                }
            }

            // Edges need to be created
            if(graphViewChange.edgesToCreate != null)
            {
                foreach(Edge edge in graphViewChange.edgesToCreate)
                {
                    QuestViewNode parentView = edge.output.node as QuestViewNode;
                    QuestViewNode childView = edge.input.node as QuestViewNode;
                    parentView.AddChildConnection(childView);
                    childView.Parent = parentView;
                    rawDataTree.ConnectNodes(parentView.questNode, childView.questNode, edge);
                }
            }

            return graphViewChange;

        }

        private void CreateNode(NodeTypes type)
        {
            if (editorReference != null)
            {
                editorReference.ChangesHaveOcurred();
            }
            QuestNodeData nodeData = new QuestNodeData();
            switch (type)
            {
                case NodeTypes.Challenge:
                case NodeTypes.Decision:
                    nodeData.buttonStrings = new string[2];
                    break;
                default:
                    nodeData.buttonStrings = new string[1];
                    break;
            }
            nodeData.type = QuestController.MapTypeToString(type);
            nodeData.key = GUID.Generate().ToString();
            rawDataTree.nodes.Add(nodeData);
            InstantiateNodeElement(nodeData);
        }

        private void InstantiateNodeElement(QuestNodeData data)
        {
            QuestViewNode viewNode = new QuestViewNode(data);
            viewNode.OnNodeSelected = OnNodeSelected;
            AddElement(viewNode);
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
