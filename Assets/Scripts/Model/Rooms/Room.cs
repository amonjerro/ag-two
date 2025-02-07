
using System.Collections.Generic;
using Tasks;
using UnityEngine;
using UnityEngine.Assertions;

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

        public abstract Task ClosestTask();

        public abstract AbsRoomClickEvent HandleClick();
        protected TaskValidator validator;

        protected void ValidateTaskType(Task task)
        {
            bool isValid = validator.Validate(task);
            Assert.IsTrue(isValid);
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

        public bool Busy { get { return _taskAssigned; } }
    }
    
    public class OperationsRoom : Room
    {
        List<PartyTask> tasks;
        public OperationsRoom()
        {
            tasks = new List<PartyTask>();
            components = new List<RoomComponent>();
            buildRestriction = BuildRestrictions.NONE;
            roomType = RoomType.OPS;
            validator = new TaskValidator();
            validator.AddValidationRule(ValidationOperations.Contains, new List<TaskType>() { TaskType.Quest, TaskType.Exploration });
        }

        public override void EnqueueTask(Task task)
        {
            ValidateTaskType(task);
            tasks.Add((PartyTask)task);
            _taskAssigned = true;
        }

        public override AbsRoomClickEvent HandleClick() {
            return new SceneTransitionEvent(roomType);
        }

        public override void RoomTick()
        {
            if (!_taskAssigned) {
                return;
            }

            List<int> indexRemoval = new List<int>();
            for (int i = 0; i < tasks.Count; i++) {
                Task task = tasks[i];
                task.HandleTick();
                // Keep track for later deletion
                if (task.IsComplete)
                {
                    indexRemoval.Add(i);
                }
            }

            // Remove done stuff from the list
            indexRemoval.Reverse();
            for (int i = indexRemoval.Count - 1; i >= 0; i--) {
                tasks.RemoveAt(indexRemoval[i]);
            }
            // Check for clear
            if (tasks.Count == 0)
            {
                _taskAssigned = false;
            }
        }

        public override Task ClosestTask()
        {
            Task task = tasks[0];
            int largestRemaining = task.RemainingTime;
            foreach (Task t in tasks) { 
                if (t.RemainingTime < largestRemaining)
                {
                    task = t;
                    largestRemaining = t.RemainingTime;
                }
            }
            return task;
        }
    }

    public class DebrisRoom : Room
    {

        Task debrisClearTask;
        public DebrisRoom()
        {
            components = new List<RoomComponent>();
            buildRestriction = BuildRestrictions.NONE;
            roomType = RoomType.DBR;
            _taskAssigned = false;
            validator = new TaskValidator();
            validator.AddValidationRule(ValidationOperations.Match, TaskType.Build);
        }

        public override void EnqueueTask(Task task)
        {
            ValidateTaskType(task);

            debrisClearTask = task;
            _taskAssigned = true;
        }

        public override void RoomTick()
        {
            if (!_taskAssigned) {
                return;
            }
            debrisClearTask.HandleTick();
            if (debrisClearTask.IsComplete) {
                debrisClearTask = null;
                _taskAssigned = false;
            }
        }

        public override AbsRoomClickEvent HandleClick() { 
            return new MenuOpenEvent(roomType);
        }

        public override Task ClosestTask()
        {
            return debrisClearTask;
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
            validator = new TaskValidator();
            validator.AddValidationRule(ValidationOperations.Match, TaskType.Build);
        }

        // Barracks can take build upgrade tasks
        public override void EnqueueTask(Task task)
        {
            ValidateTaskType(task);
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
            if (upgradeTask.IsComplete)
            {
                upgradeTask = null;
                _taskAssigned = false;
            }
        }

        public override AbsRoomClickEvent HandleClick() { 
            return new MenuOpenEvent(roomType);
        }

        public override Task ClosestTask()
        {
            return upgradeTask;
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
            validator = new TaskValidator();
            validator.AddValidationRule(ValidationOperations.Match, TaskType.Research);
        }

        public override void EnqueueTask(Task task)
        {
            ValidateTaskType(task);
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

            if (researchTask.IsComplete)
            {
                researchTask = null;
                _taskAssigned = false;
            }
        }

        public override AbsRoomClickEvent HandleClick() {
            return new MenuOpenEvent(roomType);
        }

        public override Task ClosestTask()
        {
            return researchTask;
        }
    }
}
