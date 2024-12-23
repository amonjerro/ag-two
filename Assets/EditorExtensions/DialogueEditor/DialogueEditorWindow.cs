using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
namespace DialogueEditor
{
    public class DialogueEditorWindow : EditorWindow
    {
        string activeTreeFilePath;

        DialogueTreeView treeView;
        DialogueInspector inspector;


        [MenuItem("Window/Dialogue Editor Window")]
        static void ShowEditor()
        {
            DialogueEditorWindow editor = GetWindow<DialogueEditorWindow>();
            editor.titleContent = new GUIContent("Dialogue Editor Window");
            editor.saveChangesMessage = "This conversation has unsaved changes. Would you like to save first?";

        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            // Create elements that are editable in the Unity UI Builder. (Window->UI Toolkit->UI Builder)
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/EditorExtensions/DialogueEditor/DialogueEditor.uxml");
            visualTree.CloneTree(root);

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/EditorExtensions/DialogueEditor/DialogueEditor.uss");
            root.styleSheets.Add(styleSheet);

            treeView = root.Q<DialogueTreeView>();
            inspector = root.Q<DialogueInspector>();

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
                string replacedString = fileContent.Replace("creator-version", "creatorVersion");
                DialogueNodeTree nodeTree = JsonUtility.FromJson<DialogueNodeTree>(fileContent);
                DrawDialogueStructure(nodeTree);
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
                activeTreeFilePath = EditorUtility.SaveFilePanel("Select Save Location", "", "dialogue_nodes.json", "json");
            }

            if (activeTreeFilePath.Length > 0)
            {
                string jsonString = JsonUtility.ToJson(treeView.GetNodeTree());
                File.WriteAllText(activeTreeFilePath, jsonString);
                EditorUtility.DisplayDialog("Save Successful", "Data saved successfully", "Ok");
                hasUnsavedChanges = false;
            }

        }


        private void DrawDialogueStructure(DialogueNodeTree nodeTree)
        {
            // Guard clause to prevent funny stuff
            if (nodeTree == null) return;

            treeView.PopulateView(nodeTree);

        }

        private void OnNodeSelectionChanged(DialogueViewNode node)
        {
            inspector.UpdateSelection(node);
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
            DialogueNodeTree dataArray = treeView.GetNodeTree();
            bool isValid = true;

            foreach (DialogueNodeData dnd in dataArray.passages)
            {
                if (dnd.pid == "")
                {
                    return false;
                }

                if (dnd.name == "")
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

