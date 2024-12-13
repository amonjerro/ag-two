using System;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace QuestBuilder {
    public class NodeTreeView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<NodeTreeView, UxmlTraits> { }

        QuestNodeArray rawDataTree;
        public Action<QuestViewNode> OnNodeSelected;


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

        }

        public void PopulateView(QuestNodeArray questData)
        {
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange) { 
            // Delete elements in graph view
            if(graphViewChange.elementsToRemove != null)
            {

            }

            // Edges need to be created
            if(graphViewChange.edgesToCreate != null)
            {

            }

            return graphViewChange;

        }

        private void InstantiateNodeElement(QuestNodeData data)
        {
            QuestViewNode viewNode = new QuestViewNode(data);
            viewNode.OnNodeSelected = OnNodeSelected;
            AddElement(viewNode);
        }

        
    }
}
