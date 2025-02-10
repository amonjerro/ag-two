using System;
using System.Collections.Generic;

namespace Research
{
    public enum ResearchReward
    {
        RoomType = 0
    }

    [Serializable]
    public class ResearchDataNode
    {
        public string key;
        public string title;
        public string description;
        public int cost;
        public List<string> requirements;
        public List<string> dependents;
        public Position position;
        public ResearchReward rewardType;
    }

    [Serializable]
    public class ResearchTree
    {
        public List<ResearchDataNode> nodes;
    }
}