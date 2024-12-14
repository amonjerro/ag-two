using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
namespace QuestBuilder {
    public class QuestTreeEditor : EditorWindow
    {
        string activeTreeFilePath;
        QuestNodeArray questNodeArray;

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

        }

        private void LoadTree()
        {
            activeTreeFilePath = EditorUtility.OpenFilePanel("Select Tree", "", "json");
            if (activeTreeFilePath.Length != 0)
            {
                string fileContent = File.ReadAllText(activeTreeFilePath);
                questNodeArray = JsonUtility.FromJson<QuestNodeArray>(fileContent);
                DrawQuestStructure();
            }
        }

        private void SaveTree()
        {
            if (activeTreeFilePath == "")
            {
                activeTreeFilePath = EditorUtility.SaveFilePanel("Select Save Location", "", "quest_nodes.json", "json");
            }

            if (activeTreeFilePath.Length > 0 && questNodeArray != null)
            {
                string jsonString = JsonUtility.ToJson(questNodeArray);
                File.WriteAllText(activeTreeFilePath, jsonString);
            }
        }


        private void DrawQuestStructure()
        {
            // Guard clause to prevent funny stuff
            if (questNodeArray == null) return;

            treeView.PopulateView(questNodeArray);

        }

        private void OnNodeSelectionChanged(QuestViewNode node)
        {
            inspector.UpdateSelection(node);
        }


    }
}

