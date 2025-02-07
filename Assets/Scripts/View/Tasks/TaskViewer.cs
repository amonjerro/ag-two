using UnityEngine;
using TMPro;
using SaveGame;
using Rooms;
using System.Collections.Generic;
namespace Tasks
{
    public class TaskViewer : UIPanel
    {
        RoomManager manRef;
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
        }

        private void Start()
        {
            TimeManager.Tick += UpdateDisplay;
            manRef = ServiceLocator.Instance.GetService<RoomManager>();
        }

        public override void Dismiss()
        {
        }

        public override void Show()
        {
            
        }

        private void UpdateDisplay()
        {
            List<Task> tasks = manRef.GetActiveTasks();
            if (tasks.Count == 0)
            {
                activeTaskInfo.SetActive(false);
                noTasksInfo.SetActive(true);
            }
            else
            {
                Debug.Log("Updating Display");
                activeTaskInfo.SetActive(true);
                taskName.text = tasks[0].Title;
                taskTime.text = tasks[0].RemainingTime.ToString();
                noTasksInfo.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            TimeManager.Tick -= UpdateDisplay;
        }
    }
}
