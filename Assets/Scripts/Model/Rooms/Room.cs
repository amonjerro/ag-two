
using System.Collections.Generic;
using Tasks;
using UnityEditor.Tilemaps;

namespace Rooms
{
    public enum BuildRestrictions
    {
        NONE,
        UNDERGROUND,
        ABOVE_GROUND
    }

    public enum RoomType
    {
        OPS,
        BLD,
        DBR,
        BRK,
        TRN,
        LAB,
        LIB,
        FRG,
        INF
    }

    public abstract class Room
    {
        protected bool _taskAssigned;
        protected string _roomName;
        protected string _roomDescription;
        public string Name { get { return _roomName; } set { _roomName = value; } }
        public string Description { get { return _roomDescription; } set { _roomDescription = value; } }

        public BuildRestrictions buildRestriction;
        public RoomType roomType;

        protected List<RoomComponent> components;

        public virtual void RoomTick() { }
        public abstract void EnqueueTask(Task task);

        public void AddComponent(RoomComponent component)
        {
            components.Add(component);
        }

        public abstract AbsRoomClickEvent HandleClick();

        protected void ValidateTaskType(Task task, TaskType expectedType)
        {
            // BuildTasks can be assigned to a Debris room
            if (task.TaskType != expectedType)
            {
                throw new System.ArgumentException("Incorrect Task Type assigned");
            }
        }

        public RoomComponent GetRoomComponent(ComponentType componentType) {
            foreach (RoomComponent component in components) { 
                if (component.ComponentType == componentType)
                {
                    return component;
                }
            }
            return null;
        }
    }
    
    public class OperationsRoom : Room
    {
        List<QuestTask> tasks;
        public OperationsRoom()
        {
            tasks = new List<QuestTask>();
            components = new List<RoomComponent>();
            buildRestriction = BuildRestrictions.NONE;
            roomType = RoomType.OPS;
        }

        public override void EnqueueTask(Task task)
        {
            if (task.TaskType != TaskType.Quest)
            {
                throw new System.ArgumentException("This type of task can't be set to the operations room");
            }
            
            tasks.Add((QuestTask)task);
        }

        public override AbsRoomClickEvent HandleClick() {
            return new SceneTransitionEvent(roomType);
        }

        public override void RoomTick()
        {
            foreach (QuestTask task in tasks) { 
                task.HandleTick();
            }
        }
    }

    public class DebrisRoom : Room
    {

        BuildTask debrisClearTask;
        public DebrisRoom()
        {
            components = new List<RoomComponent>();
            buildRestriction = BuildRestrictions.NONE;
            roomType = RoomType.DBR;
            _taskAssigned = false;
        }

        public override void EnqueueTask(Task task)
        {
            ValidateTaskType(task, TaskType.Build);

            debrisClearTask = (BuildTask)task;
            _taskAssigned = true;
        }

        public override void RoomTick()
        {
            if (!_taskAssigned) {
                return;
            }

            debrisClearTask.HandleTick();
        }

        public override AbsRoomClickEvent HandleClick() { 
            return new MenuOpenEvent(roomType);
        }

    }

    public class Barracks : Room
    {
        BuildTask upgradeTask;
        public Barracks()
        {
            components = new List<RoomComponent>();
            roomType = RoomType.BRK;
            buildRestriction = BuildRestrictions.ABOVE_GROUND;
            _taskAssigned = false;
        }

        // Barracks can take build upgrade tasks
        public override void EnqueueTask(Task task)
        {
            ValidateTaskType(task, TaskType.Build);
            upgradeTask = (BuildTask)task;
            _taskAssigned = true;
        }

        public override void RoomTick()
        {
            if (!_taskAssigned)
            {
                return;
            }

            upgradeTask.HandleTick();
        }

        public override AbsRoomClickEvent HandleClick() { 
            return new MenuOpenEvent(roomType);
        }
    }
    public class LibraryRoom : Room
    {
        ResearchTask researchTask;

        public LibraryRoom()
        {
            components = new List<RoomComponent>();
            roomType = RoomType.LIB;
            buildRestriction = BuildRestrictions.NONE;
            _taskAssigned = false;
        }

        public override void EnqueueTask(Task task)
        {
            ValidateTaskType(task, TaskType.Research);
            researchTask = (ResearchTask)task;
            _taskAssigned = true;
        }

        public override void RoomTick()
        {
            if (!_taskAssigned)
            {
                return;
            }

            researchTask.HandleTick();
        }

        public override AbsRoomClickEvent HandleClick() {
            return new MenuOpenEvent(roomType);
        }
    }
}
