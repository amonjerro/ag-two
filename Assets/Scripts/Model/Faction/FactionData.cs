using System.Collections.Generic;

public static class FactionData
{
    static Dictionary<Faction, int> reputation = new Dictionary<Faction, int>();
    static Dictionary<Faction, int> completedQuests = new Dictionary<Faction, int>();
}

public enum Faction
{
    KnightsCircle,
    Consortium,
    Dungeoneers,
    Loremasters,
    VerdantGrove
}