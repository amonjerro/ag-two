using System.Collections.Generic;

public enum NodeTypes
{
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
    public int Duration { get; set; }
    public int CurrentTickCount { get; set; }

    public AQuestNode Next { get; set; }

    protected abstract void Start();
    protected abstract void End();

    public void OnStart(QuestState state)
    {
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
    protected override void Start()
    {

    }

    protected override void End()
    {

    }
}

public class DecisionNode : AQuestNode
{
    public AQuestNode Option1 { get; set; }
    public AQuestNode Option2 { get; set; }

    public AQuestNode Decision;


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
