using UnityEditor;

namespace ExplorationEventsTool
{
    public class ExplorationEventsEditor : EditorWindow
    {
        ExplorationEventsControls eventControls;
        ExplorationEventsInspector eventInspector;

        [MenuItem("Window/Exploration Events Editor")]
        static void ShowEditor()
        {
            ExplorationEventsEditor explorationEventsEditor = GetWindow<ExplorationEventsEditor>();
            explorationEventsEditor.titleContent = new UnityEngine.GUIContent("Event Editor");
            explorationEventsEditor.saveChangesMessage = "Some events have changed but have not been saved. Would you like to save your work?";
        }

    }
}
