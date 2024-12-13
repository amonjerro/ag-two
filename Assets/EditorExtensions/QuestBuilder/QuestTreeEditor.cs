using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class QuestTreeEditor : EditorWindow
{
    string activeTreeFilePath;
    QuestNodeArray questNodeArray;

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

        activeTreeFilePath = "";

        Button button = new Button();
        button.clicked += () => LoadTree();
        button.name = "load";
        button.text = "Load Tree";
        root.Add(button);

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

        if (activeTreeFilePath.Length > 0)
        {
            string jsonString = JsonUtility.ToJson(questNodeArray);
            File.WriteAllText(activeTreeFilePath, jsonString);
        }
    }


    private void DrawQuestStructure()
    {
        // Guard clause to prevent funny stuff
        if (questNodeArray == null) return;



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
