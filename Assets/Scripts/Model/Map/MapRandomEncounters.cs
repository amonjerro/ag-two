using ExplorationMap;
using System;
using System.Collections.Generic;

public class EncounterOutcome
{

}


[Serializable]
public class MapEncounter
{
    public string title;
    public string desc;
    public List<EncounterOutcome> outcomes;

    public string imagePath;

    // Condtionals
    public bool bRequiresConnectionType;
    public ConnectionType requiredConnectionType;
    public int minTickDelay;
    public int maxTickDelay;
    public bool isUnique;

}