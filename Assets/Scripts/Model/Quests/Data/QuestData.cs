using System.Collections.Generic;

public class QuestData
{
    public Faction faction;
    public string questKey;
    public string questTitle;
    public string description;
    public int startTick;
    public bool hidden = false;

    public int SOIndex { get; set; }
    public bool HasBeenCompleted { get; private set; }

    public AQuestNode rootNode;


    public QuestData(Faction faction, string questKey, string questTitle, string description, int startTick, int sOIndex)
    {
        this.faction = faction;
        this.questKey = questKey;
        this.questTitle = questTitle;
        this.description = description;
        this.startTick = startTick;
        SOIndex = sOIndex;
    }
}