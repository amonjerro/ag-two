using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
namespace QuestBuilder
{
    public class FactionQuestEditor : EditorWindow
    {
        string activeTreeFilePath;

        FactionQuestTree treeView;
        FactionQuestInspector inspector;


        [MenuItem("Window/Faction Quest Editor")]
        static void ShowEditor()
        {
            FactionQuestEditor editor = GetWindow<FactionQuestEditor>();
            editor.titleContent = new GUIContent("Faction Quest Editor");
            editor.saveChangesMessage = "This quest has unsaved changes. Would you like to save first?";

        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            // Create elements that are editable in the Unity UI Builder. (Window->UI Toolkit->UI Builder)
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/EditorExtensions/QuestBuilder/FactionQuestBuilder.uxml");
            visualTree.CloneTree(root);

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/EditorExtensions/QuestBuilder/QuestBuilder.uss");
            root.styleSheets.Add(styleSheet);

            treeView = root.Q<FactionQuestTree>();
            inspector = root.Q<FactionQuestInspector>();

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
                FactionQuestTreeData questNodeArray = JsonUtility.FromJson<FactionQuestTreeData>(fileContent);
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


        private void DrawQuestStructure(FactionQuestTreeData questNodes)
        {
            // Guard clause to prevent funny stuff
            if (questNodes == null) return;

            treeView.PopulateView(questNodes);

        }

        private void OnNodeSelectionChanged(GraphTreeNode node)
        {
            FactionViewNode factionViewNode = node as FactionViewNode;
            inspector.UpdateSelection(factionViewNode);
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
            FactionQuestTreeData dataArray = treeView.GetNodeTree();
            bool isValid = true;

            foreach (FactionQuestTreeNode node in dataArray.nodes)
            {
                if (node.questKey == "")
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

