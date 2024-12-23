using Rooms;

namespace Tasks {
    public enum TaskType
    {
        Research,
        Build,
        Quest,
        Forge
    }

    public abstract class Task
    {
        protected TaskType _task;
        public TaskType TaskType { get { return _task; } }

        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int Duration {  get; set; }
        public int TimeElapsed {  get; set; }

        public virtual void HandleTick()
        {
            TimeElapsed++;
            if (TimeElapsed >= Duration)
            {
                Complete();
            }
        }
        
        protected abstract void OnComplete();

        public virtual void Complete() {
            OnComplete();
        }
    }

    public class ResearchTask : Task
    {

        protected override void OnComplete()
        {
            throw new System.NotImplementedException();
        }
    }

    public class BuildTask : Task
    {
        public BuildTask() {
            _task = TaskType.Build;
        }
        public (int, int) RoomCoordinates {  get; set; }
        public RoomType TypeBeingBuilt { get; set; }

        protected override void OnComplete()
        {
            BuildCompleteNotify buildCompleteNotify = new BuildCompleteNotify();
            buildCompleteNotify.coordinates = RoomCoordinates;
            buildCompleteNotify.roomType = TypeBeingBuilt;
            TaskNotifyQueue.AddTaskNotification(buildCompleteNotify);
        }
    }

    public class QuestTask : Task
    {
        public AQuestNode questNode { get; set; }
        protected override void OnComplete()
        {
            QuestNodeNotify questNodeNotify = new QuestNodeNotify();
            questNodeNotify.SetQuestNode(questNode);
            TaskNotifyQueue.AddTaskNotification(questNodeNotify);
        }
    }

    public class ForgeTask : Task
    {
        protected override void OnComplete()
        {
            throw new System.NotImplementedException(); 
        }
    }
}

