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

        protected override void OnComplete()
        {
            throw new System.NotImplementedException();
        }
    }

    public class QuestTask : Task
    {
        protected override void OnComplete()
        {
            throw new System.NotImplementedException();
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

