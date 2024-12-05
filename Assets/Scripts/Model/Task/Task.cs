namespace Tasks {
    public abstract class Task
    {
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int Duration {  get; set; }
        protected abstract void OnComplete();

        public void Complete() {
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

