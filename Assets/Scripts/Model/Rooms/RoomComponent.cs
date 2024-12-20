using System.Collections.Generic;

namespace Rooms
{
    public enum ComponentType
    {
        BUILDABLE,
        UPGRADEABLE,
        HOLDS_ADVENTURERS
    }

    public abstract class RoomComponent
    {
        protected Room parentRoom;
        protected ComponentType _type;
        public ComponentType ComponentType { get { return _type; } protected set { _type = value; }  }
        public abstract bool MaxedOut { get; }

        public abstract int GetCost();
        public abstract void Grow();
        public abstract void Grow(int toWhere);
    }

    public class BuildableComponent : RoomComponent
    {
        int buildCost;
        public override bool MaxedOut { get { return false; } }
        public BuildableComponent(int cost)
        {
            ComponentType = ComponentType.BUILDABLE;
            buildCost = cost;
        }

        public override int GetCost()
        {
            return buildCost;
        }

        public override void Grow(int toWhere)
        {
            
        }

        public override void Grow()
        {
           
        }

    }

    public class HoldComponent : RoomComponent
    {
        private List<int> capacityLevels;
        private bool maxedOut;
        private int currentLevel;
        private int occupancy;
        private int costPerOccupancy;

        public override bool MaxedOut { get { return maxedOut; } }

        public HoldComponent(int costPerOccupancy, int capacityCount, int growthFactor)
        {
            ComponentType = ComponentType.HOLDS_ADVENTURERS;
            maxedOut = false;
            currentLevel = 0;
            occupancy = 0;
            capacityLevels = new List<int>();
            this.costPerOccupancy = costPerOccupancy;
            PopulateHoldList(capacityCount, growthFactor);

        }

        private void PopulateHoldList(int capacityCount, int growthFactor)
        {
            for (int i = 1; i <= capacityCount; i++) {
                capacityLevels.Add(i * growthFactor);
            }
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

        public UpgradeableComponent(int baseCost, int count, int costFactor)
        {
            ComponentType = ComponentType.UPGRADEABLE;
            currentLevel = 0;
            maxedOut = false;
            upgradeCosts = new List<int>();
            ExpandCostList(baseCost, count, costFactor);
        }

        private void ExpandCostList(int baseCost, int count, int factor)
        {
            for (int i = 0; i < count; i++) {
                upgradeCosts.Add(baseCost + (factor * i));
            }
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