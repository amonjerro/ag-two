using ExplorationMap;
using System;
using System.Collections.Generic;


[Serializable]
public enum OutcomeType
{
    Reward = 0,
    StartQuest
}


[Serializable]
public class EncounterOutcome
{
    OutcomeType type;
    string outcomeText;
    string outcomeTitle;
    Reward reward;
    string questKey;
}


[Serializable]
public class MapEncounter
{
    public string title;
    public string desc;
    public List<EncounterOutcome> outcomes;

    public string imagePath;

    // Conditionals
    public bool bRequiresConnectionType;
    public ConnectionType requiredConnectionType;
    public int minTickDelay;
    public int maxTickDelay;
    public bool isUnique;

}