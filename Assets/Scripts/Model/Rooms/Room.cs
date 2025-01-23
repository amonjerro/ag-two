
using System;
using System.Collections.Generic;
using Tasks;
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
            validator.AddValidationRule(ValidationOperations.Match, TaskType.Party);
        }

        public override void EnqueueTask(Task task)
        {
            ValidateTaskType(task);
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
        }

        public override AbsRoomClickEvent HandleClick() {
            return new MenuOpenEvent(roomType);
        }
    }
}
