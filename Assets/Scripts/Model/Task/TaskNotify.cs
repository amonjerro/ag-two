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

        public override void Show()
        {
            throw new System.NotImplementedException();
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