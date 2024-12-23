using Rooms;

namespace Tasks
{
    public abstract class TaskNotify
    {
        public Task task {  get; set; }
        public abstract void Show();
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
        public override void Show()
        {
            
        }

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

        public override void Show()
        {

        }

        public override void OnAcknowledge()
        {
            QuestController qc = ServiceLocator.Instance.GetService<AdventurerManager>().GetQuestController();
            qc.ProceedToNext(node);
        }
    }
}