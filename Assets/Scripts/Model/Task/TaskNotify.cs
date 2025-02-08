using Rooms;
using SaveGame;
using UnityEngine;

namespace Tasks
{
    public abstract class TaskNotify
    {
        public Task task {  get; set; }
        public abstract void OnAcknowledge();

        public void Acknowledge()
        {
            TaskNotifyQueue.AcknowledgeNotification();
            OnAcknowledge();
        }
    }

    public class BuildCompleteNotify : TaskNotify
    {
        public (int, int) coordinates { get; set; }
        public RoomType roomType {get; set; } 


        public override void OnAcknowledge()
        {
           
        }
    }

    public class QuestNodeNotify : TaskNotify
    {
        AQuestNode node;

        public void SetQuestNode(AQuestNode node)
        {
            this.node = node;
        }

        public override void OnAcknowledge()
        {
            QuestController qc = ServiceLocator.Instance.GetService<AdventurerManager>().GetQuestController();
            qc.ProceedToNext(node);
        }
    }

    public class ExplorationTaskNotify : TaskNotify
    {
        public override void OnAcknowledge()
        {
            
            ExplorationTask exploration = (ExplorationTask)task;
            // Endure the player's progress
            GameInstance.exploredCoordinates.Add(exploration.GetCoordinates());

            // Return the adventurers to not on mission
            exploration.ReleaseAdventurers();
        }
    }
}