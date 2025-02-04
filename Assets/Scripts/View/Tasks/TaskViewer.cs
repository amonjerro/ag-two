using UnityEngine;
using TMPro;
using SaveGame;
namespace Tasks
{
    public class TaskViewer : UIPanel
    {
        [SerializeField]
        GameObject activeTaskInfo;

        [SerializeField]
        GameObject noTasksInfo;

        [SerializeField]
        TextMeshProUGUI taskName;

        [SerializeField]
        TextMeshProUGUI taskTime;

        private void Awake()
        {
            GameInstance.tasksUpdated += UpdateDisplay;
        }

        public override void Dismiss()
        {
            throw new System.NotImplementedException();
        }

        public override void Show()
        {
            
        }

        private void UpdateDisplay()
        {
            if (GameInstance.activeTasks.Count == 0)
            {
                activeTaskInfo.SetActive(false);
                noTasksInfo.SetActive(true);
            }
            else
            {
                activeTaskInfo.SetActive(true);
                taskName.text = GameInstance.activeTasks[0].Title;
                taskTime.text = GameInstance.activeTasks[0].RemainingTime.ToString();
                noTasksInfo.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            GameInstance.tasksUpdated -= UpdateDisplay;
        }
    }
}
