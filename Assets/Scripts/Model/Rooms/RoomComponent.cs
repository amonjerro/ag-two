using System.Collections.Generic;

namespace Rooms
{
    public enum ComponentType
    {
        UPGRADEABLE,
        HOLDS_ADVENTURERS
    }

    public abstract class RoomComponent
    {
        protected ComponentType _type;
        public ComponentType ComponentType { get { return _type; } protected set { _type = value; }  }
        public abstract bool MaxedOut { get; }

        public abstract int GetCost();
        public abstract void Grow();
        public abstract void Grow(int toWhere);
    }

    public class HoldComponent : RoomComponent
    {
        private List<int> capacityLevels;
        private bool maxedOut;
        private int currentLevel;
        private int occupancy;
        private int costPerOccupancy;

        public override bool MaxedOut { get { return maxedOut; } }

        public HoldComponent(List<int> capacityLevels, int costPerOccupancy)
        {
            ComponentType = ComponentType.HOLDS_ADVENTURERS;
            maxedOut = false;
            currentLevel = 0;
            occupancy = 0;
            this.capacityLevels = capacityLevels;
            this.costPerOccupancy = costPerOccupancy;   
        }

        public override int GetCost()
        {
            return costPerOccupancy * occupancy;
        }

        public override void Grow()
        {
            currentLevel++;
            if (currentLevel >= capacityLevels.Count)
            {
                maxedOut = true;
            }
        }

        public override void Grow(int toWhere)
        {
            currentLevel = toWhere;
            if (currentLevel >= capacityLevels.Count)
            {
                maxedOut = true;
            }
        }
    }


    public class UpgradeableComponent : RoomComponent
    {
        private List<int> upgradeCosts;
        private int currentLevel;
        private bool maxedOut;

        public override bool MaxedOut { get { return maxedOut; } }

        public UpgradeableComponent(List<int> upgradeCosts)
        {
            ComponentType = ComponentType.UPGRADEABLE;
            currentLevel = 0;
            maxedOut = false;
            this.upgradeCosts = upgradeCosts;
        }

        public override int GetCost()
        {
            if (maxedOut) {
                return -1;
            }

            return upgradeCosts[currentLevel];

        }

        public override void Grow()
        {
            currentLevel++;
            if (currentLevel >= upgradeCosts.Count) { 
                maxedOut = true;
            }
        }

        public override void Grow(int toWhere)
        {
            currentLevel = toWhere;
            if (currentLevel >= upgradeCosts.Count) {
                maxedOut = true;
            }
        }
    }
}