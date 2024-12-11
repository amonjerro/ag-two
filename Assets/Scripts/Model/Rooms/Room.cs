
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
        BRK,
        TRN,
        LAB,
        LIB,
        FRG,
        INF
    }

    public abstract class Room
    {

        public abstract string Name { get; }

        public BuildRestrictions buildRestriction;
        public RoomType roomType;

        protected List<RoomComponent> components;

        public abstract void RoomTick();
        public abstract void EnqueueTask(Task task);

        public void AddComponent(RoomComponent component)
        {
            components.Add(component);
        }

    }


}
