using Rooms;

namespace Tasks
{
    public static class TaskFactory
    {
        public static Task MakeTask(TaskType type)
        {
            switch (type)
            {
                case TaskType.Build:
                    BuildTask buildTask = new BuildTask();
                    buildTask.Duration = 24;
                    return buildTask;
            }
            return null;
        }

        public static BuildTask MakeBuildTask((int, int) roomCoordinates, RoomType typeBeingBuilt)
        {
            BuildTask buildTask = new BuildTask();
            buildTask.RoomCoordinates = roomCoordinates;
            buildTask.TypeBeingBuilt = typeBeingBuilt;
            buildTask.Duration = 24;

            return buildTask;
        }
    }
}
