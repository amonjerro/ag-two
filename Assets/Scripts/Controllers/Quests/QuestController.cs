using System.Collections.Generic;
using UnityEngine;

public partial class QuestController
{
    List<QuestData> openQuestData;
    List<QuestData> closedQuestData;
    List<AQuestNode> currentlyActiveQuests;
    public bool QuestListEmpty { get { return currentlyActiveQuests.Count == 0; } }

    public void Tick()
    {
        // Check to see if there are any active quests
        if (currentlyActiveQuests.Count == 0 && openQuestData.Count == 0)
        {
            return;
        }

    }

    public QuestController(List<SO_QuestData> questDataObjects) { 
        currentlyActiveQuests = new List<AQuestNode>();
        openQuestData = new List<QuestData>();
        closedQuestData = new List<QuestData>();

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


    public void AddQuest()
    {

    }

    public List<QuestData> GetOpenQuests()
    {
        return openQuestData;
    }

    ~QuestController()
    {
        TimeManager.Tick -= Tick;
    }
}