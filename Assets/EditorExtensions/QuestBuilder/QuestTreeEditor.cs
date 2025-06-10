using System.IO;

using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
namespace QuestBuilder {
    public class QuestTreeEditor : EditorWindow
    {
        string activeTreeFilePath;
        NodeTreeView treeView;
        NodeInspector inspector;


        [MenuItem("Window/Quest Tree Editor")]
        static void ShowEditor()
        {
            QuestTreeEditor editor = GetWindow<QuestTreeEditor>();
            editor.titleContent = new GUIContent("Quest Tree Editor");
            editor.saveChangesMessage = "This quest has unsaved changes. Would you like to save first?";
            
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            // Create elements that are editable in the Unity UI Builder. (Window->UI Toolkit->UI Builder)
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/EditorExtensions/QuestBuilder/QuestBuilder.uxml");
            visualTree.CloneTree(root);

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/EditorExtensions/QuestBuilder/QuestBuilder.uss");
            root.styleSheets.Add(styleSheet);

            treeView = root.Q<NodeTreeView>();
            inspector = root.Q<NodeInspector>();

            Button newButton = new Button();
            newButton.text = "New Tree";
            newButton.clicked += NewTree;
            inspector.Add(newButton);

            Button loadButton = new Button();
            loadButton.text = "Load Tree";
            loadButton.clicked += LoadTree;
            inspector.Add(loadButton);

            Button saveButton = new Button();
            saveButton.text = "Save Tree";
            saveButton.clicked += SaveTree;
            inspector.Add(saveButton);

            inspector.Init();
            treeView.OnNodeSelected = OnNodeSelectionChanged;
            treeView.SetEditorReference(this);
            activeTreeFilePath = "";
        }

        private void NewTree()
        {
            activeTreeFilePath = "";
            treeView.ClearTree();
            treeView.InitializeTree();
        }

        private void LoadTree()
        {
            activeTreeFilePath = EditorUtility.OpenFilePanel("Select Tree", "", "json");
            if (activeTreeFilePath.Length != 0)
            {
                string fileContent = File.ReadAllText(activeTreeFilePath);
                QuestNodeArray questNodeArray = JsonUtility.FromJson<QuestNodeArray>(fileContent);
                DrawQuestStructure(questNodeArray);
            }
        }

        private void SaveTree()
        {
            // Validation
            if (!NodeDataHasValidIntegrity())
            {
                EditorUtility.DisplayDialog("Data Integrity Error", "There are nodes in this tree with missing data", "Ok");
                return;
            }

            if (activeTreeFilePath == "")
            {
                activeTreeFilePath = EditorUtility.SaveFilePanel("Select Save Location", "", "quest_nodes.json", "json");
            }

            if (activeTreeFilePath.Length > 0)
            {
                string jsonString = JsonUtility.ToJson(treeView.GetNodeTree());
                File.WriteAllText(activeTreeFilePath, jsonString);
                EditorUtility.DisplayDialog("Save Successful", "Data saved successfully", "Ok");
                hasUnsavedChanges = false;
            }
            
        }


        private void DrawQuestStructure(QuestNodeArray questNodes)
        {
            // Guard clause to prevent funny stuff
            if (questNodes == null) return;

            treeView.PopulateView(questNodes);

        }

        private void OnNodeSelectionChanged(GraphTreeNode node)
        {
            QuestViewNode questViewNode = (QuestViewNode)node;
            inspector.UpdateSelection(questViewNode);
        }


        public override void SaveChanges()
        {
            base.SaveChanges();
            SaveTree();
        }

        public override void DiscardChanges()
        {
            base.DiscardChanges();
        }

        private bool NodeDataHasValidIntegrity()
        {
            QuestNodeArray dataArray = treeView.GetNodeTree();
            bool isValid = true;

            foreach(QuestNodeData qnd in dataArray.nodes)
            {
                if (qnd.key == "")
                {
                    return false;
                }
            }

            return isValid;
        }

        public void ChangesHaveOcurred()
        {
            hasUnsavedChanges = true;
        }

    }
}

