using System.Collections.Generic;

public enum NodeTypes
{
    Start,
    Info,
    Challenge,
    Decision
}

public struct QuestState
{
    public List<Adventurer> adventurers;
}

public abstract class AQuestNode
{
    public NodeTypes NodeType { get; set; }
    protected QuestState _currentState;
    public string Title { get; set; }
    public string Description { get; set; }

    public string QuestKey { get; set; }
    public int Duration { get; set; }
    public int CurrentTickCount { get; set; }

    public AQuestNode Next { get; set; }

    public bool PendingReview { get; set; }

    protected abstract void Start();
    protected abstract void End();
    public abstract void SetButtonStrings(string[] buttonStrings);

    public void OnStart(QuestState state)
    {
        PendingReview = false;
        _currentState = state;
        Start();
    }

    public AQuestNode OnEnd()
    {
        End();
        return Next;
    }
}

public class InformationNode : AQuestNode
{
    public string AcknowledgeString { get; protected set; }

    public override void SetButtonStrings(string[] buttonStrings)
    {
        AcknowledgeString = buttonStrings[0];
    }

    protected override void Start()
    {

    }

    protected override void End()
    {

    }
}

public class DecisionNode : AQuestNode
{
    public string Option1String { get; protected set; }
    public string Option2String { get; protected set; }
    public AQuestNode Option1 { get; set; }
    public AQuestNode Option2 { get; set; }

    public AQuestNode Decision;


    public override void SetButtonStrings(string[] buttonStrings)
    {
        Option1String = buttonStrings[0];
        Option2String = buttonStrings[1];
    }

    protected override void Start()
    {

    }

    public virtual void Decide(bool value)
    {
        Decision = Option1;

        if (!value)
        {
            Decision = Option2;
        }
    }

    protected override void End()
    {
        Next = Decision;
    }
}

//public class ChallengeNode : DecisionNode
//{
//    Stats combinedPartyStats;
//    Stats challengeStats;

//    public ChallengeNode(Stats challenge)
//    {
//        challengeStats = challenge;
//    }

//    protected override void Start()
//    {
//        combinedPartyStats = new Stats();
//        for (int i = 0; i < _currentState.adventurers.Count; i++)
//        {
//            combinedPartyStats.Add(_currentState.adventurers[i].Char_Stats);
//        }
//    }

//    protected override void End()
//    {
//        throw new System.NotImplementedException();
//    }
//}
