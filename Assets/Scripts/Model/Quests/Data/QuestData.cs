using System.Collections.Generic;

public class QuestData
{
    public Faction faction;
    public string questKey;
    public string questTitle;
    public string description;
    public int startTick;
    public bool hidden = false;
    private bool _completed;
    private List<Adventurer> adventurers;

    public int SOIndex { get; set; }
    public bool HasBeenCompleted { get { return _completed; } set { _completed = value; } }

    public AQuestNode rootNode;

    public QuestData(Faction faction, string questKey, string questTitle, string description, int startTick, int sOIndex)
    {
        this.faction = faction;
        this.questKey = questKey;
        this.questTitle = questTitle;
        this.description = description;
        this.startTick = startTick;
        SOIndex = sOIndex;
        HasBeenCompleted = false;
    }

    public void BindAdventurers(List<Adventurer> adventurers)
    {
        this.adventurers = adventurers;
    }

    public List<Adventurer> GetAdventurers()
    {
        return this.adventurers;
    }

    public override string ToString()
    {
        return questTitle;
    }
}