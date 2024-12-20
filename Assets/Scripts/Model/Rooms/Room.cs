
using System.Collections.Generic;
using Tasks;

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
    }
    
    public class OperationsRoom : Room
    {
        public OperationsRoom()
        {
            components = new List<RoomComponent>();
            buildRestriction = BuildRestrictions.NONE;
            roomType = RoomType.OPS;
        }

        public override void EnqueueTask(Task task)
        {
            // No tasks can be assigned to the operations room

        }

        public override AbsRoomClickEvent HandleClick() {
            return new SceneTransitionEvent(roomType);
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
            // BuildTasks can be assigned to a Debris room
            if (task.TaskType != TaskType.Build)
            {
                throw new System.ArgumentException("Incorrect Task Type assigned");
            }

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

}
