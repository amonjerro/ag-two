using Rooms;
using System.Collections.Generic;

namespace Tasks {
    public enum TaskType
    {
        Research,
        Build,
        Quest,
        Forge,
        Exploration
    }

    public abstract class Task
    {
        protected TaskType _task;
        protected bool _paused;
        public TaskType TaskType { get { return _task; } }

        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int Duration {  get; set; }
        public int TimeElapsed {  get; set; }

        public bool IsComplete { get { return TimeElapsed > Duration; } }

        public bool IsPaused { get { return _paused; } }

        public int RemainingTime { get { return Duration - TimeElapsed; } }

        public virtual void HandleTick()
        {
            if (_paused)
            {
                return;
            }

            TimeElapsed++;
            if (IsComplete)
            {
                Complete();
            }
        }

        public void Unpause()
        {
            _paused = false;
        }
        
        protected abstract void OnComplete();

        public virtual void Complete() {
            OnComplete();
        }

        public override string ToString()
        {
            return _task.ToString();
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

    public class ForgeTask : Task
    {
        protected override void OnComplete()
        {
            throw new System.NotImplementedException();
        }
    }

    public class PartyTask : Task
    {
        protected List<Adventurer> adventurers;

        public PartyTask() {
            adventurers = new List<Adventurer>();
            TimeElapsed = 0;
        }

        public void Reset()
        {
            adventurers.Clear();
        }

        public void AddPartyMember(Adventurer adventurer)
        {
            adventurers.Add(adventurer);
        }

        public void ReleaseAdventurers()
        {
            foreach (Adventurer adventurer in adventurers)
            {
                adventurer.ReturnFromMission();

            }
        }

        protected override void OnComplete() {

        }
    }

    public class QuestTask : PartyTask
    {
        public AQuestNode questNode { get; set; }

        public QuestTask() : base() { }

        protected override void OnComplete()
        {
            QuestNodeNotify questNodeNotify = new QuestNodeNotify();
            questNodeNotify.SetQuestNode(questNode);
            TaskNotifyQueue.AddTaskNotification(questNodeNotify);
        }
    }

    public class ExplorationTask : PartyTask
    {
        (int, int) locationCoordinates;
        List<Adventurer> assignedAdventurers;
        public ExplorationTask() : base() { 
            assignedAdventurers = new List<Adventurer>();
            _task = TaskType.Exploration;
        }

        public void SetCoordinates((int, int) coordinates)
        {
            locationCoordinates = coordinates;
        }

        public (int, int) GetCoordinates()
        {
            return locationCoordinates;
        }

        public void SetAssignedAdventurers(List<Adventurer> adventurers)
        {
            assignedAdventurers = adventurers;
            foreach (Adventurer a in assignedAdventurers) {
                a.SendOnMission();
            }
        }

        protected override void OnComplete()
        {
            ExplorationTaskNotify taskNotify = new ExplorationTaskNotify();
            taskNotify.task = this;
            TaskNotifyQueue.AddTaskNotification(taskNotify);
        }
    }
}

