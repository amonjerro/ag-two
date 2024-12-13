using System.Collections.Generic;

public partial class QuestController
{
    // Data
    List<QuestData> openQuestData;
    List<QuestData> closedQuestData;
    List<AQuestNode> currentlyActiveQuests;
    Dictionary<string, QuestData> questMap;
    Queue<AQuestNode> notifications;

    // View
    NotificationPill pill;

    public static NodeTypes MapStringToType(string type)
    {
        switch (type)
        {
            case "start":
                return NodeTypes.Start;
            case "end": 
                return NodeTypes.End;
            case "skill":
                return NodeTypes.Challenge;
            case "dec":
                return NodeTypes.Decision; 
            default:
                return NodeTypes.Info;
        }
    }

    public bool NotificationsEmpty { get { return notifications.Count == 0; } }

    public void Tick()
    {
        // Check to see if there are any active quests
        if (currentlyActiveQuests.Count == 0 && openQuestData.Count == 0)
        {
            return;
        }

        bool notify = false;
        bool notificationsEmpty = NotificationsEmpty;
        List<int> nodesToRemove = new List<int>();
        int counter = 0;

        // Uptick every open quest
        foreach (AQuestNode questNode in currentlyActiveQuests)
        {
            questNode.CurrentTickCount += 1;

            if(questNode.CurrentTickCount >= questNode.Duration)
            {
                notify = true;
                // Inform upwards that an event is pertinent with regards to this quest
                questNode.PendingReview = true;
                nodesToRemove.Add(counter);

            }
            counter++;
        }

        if (notify)
        {
            // For every node that has been flagged as notification, remove it from the list
            // and move it to the notifications queue
            for (int i = nodesToRemove.Count - 1; i >= 0; i--)
            {
                AQuestNode removedNode = currentlyActiveQuests[i];
                currentlyActiveQuests.RemoveAt(i);
                notifications.Enqueue(removedNode);
            }

            if (notificationsEmpty)
            {
                pill.Show();
            } else
            {
                pill.UpdateNotification();
            }
        }
    }

    public QuestController(List<SO_QuestData> questDataObjects, NotificationPill pill) { 
        currentlyActiveQuests = new List<AQuestNode>();
        openQuestData = new List<QuestData>();
        questMap = new Dictionary<string, QuestData>();
        closedQuestData = new List<QuestData>();
        notifications = new Queue<AQuestNode>();
        this.pill = pill;

        LoadQuests(questDataObjects);
        TimeManager.Tick += Tick;
    }

    public void LoadQuests(List<SO_QuestData> data)
    {
        // Check stored memory for completed quests
        List<QuestData> rawQuestData = new List<QuestData>();
        List<QuestData> nonCompletedData = new List<QuestData>();

        for(int i = 0; i < data.Count; i++)
        {
            SO_QuestData soqd = data[i];
            QuestData qd = new QuestData(
                soqd.faction,
                soqd.questKey,
                soqd.questTitle,
                soqd.description,
                soqd.startTick,
                i
            );

            questMap.Add(soqd.questKey, qd);
            nonCompletedData.Add(qd);
        }

        // Build remaining data
        for(int i = 0; i < nonCompletedData.Count; i++)
        {
            // Load quest nodes from JSON
            QuestLoader.LoadQuest(data[nonCompletedData[i].SOIndex], nonCompletedData[i]);
        }

        openQuestData = nonCompletedData;
    }


    public void ProceedToNext(AQuestNode node)
    {
        StopActiveQuest(node);
        node.OnEnd();
        AQuestNode next = node.Next;
        if (next == null)
        {
            return;
        }

        AddActiveQuest(next);
    }

    public void DecideNode(AQuestNode node, bool val)
    {
        DecisionNode decNode = (DecisionNode)node;
        decNode.Decide(val);
        
        ProceedToNext(node);
    }

    public void AddActiveQuest(AQuestNode quest)
    {
        QuestData data = questMap[quest.QuestKey];
        QuestState state = new();
        state.adventurers = data.GetAdventurers();
        quest.OnStart(state);
        currentlyActiveQuests.Add(quest);
    }

    public AQuestNode GetNotification() {
        return notifications.Dequeue();
    }

    public void StopActiveQuest(AQuestNode quest)
    {
        currentlyActiveQuests.Remove(quest);
    }

    public List<QuestData> GetOpenQuests()
    {
        return openQuestData;
    }

    public QuestData GetQuestByKey(string key)
    {
        return questMap[key];
    }

    public void BindStagingToQuest(QuestData quest, Dictionary<int, Adventurer> staging)
    {
        List<Adventurer> questParty = new List<Adventurer>();
        foreach(KeyValuePair<int, Adventurer> kvp in staging)
        {
            questParty.Add(kvp.Value);
        }
        quest.BindAdventurers(questParty);
    }

    ~QuestController()
    {
        TimeManager.Tick -= Tick;
    }
}