using System.Collections.Generic;

public static class FactionData
{
    static Dictionary<Faction, int> reputation = new Dictionary<Faction, int>();
    static Dictionary<Faction, int> completedQuests = new Dictionary<Faction, int>();
    static Dictionary<(int, int), Faction> factionHeadquarters = new Dictionary<(int, int), Faction>();
}

[System.Serializable]
public enum Faction
{
    KnightsCircle,
    Consortium,
    Dungeoneers,
    Loremasters,
    VerdantGrove
}