using System.Collections.Generic;
using UnityEngine;

public enum NodeTypes
{
    Start,
    End,
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

    public List<AQuestNode> Children { get; set; }

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

    public AQuestNode()
    {
        Children = new List<AQuestNode>();
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

public enum ChallengeDifficulties
{
    Basic,
    Novice,
    Adept,
    Veteran,
    Expert,
    Heroic
}

public class ChallengeNode : DecisionNode
{
    Stats combinedPartyStats;
    Stats challengeStats;
    ChallengeDifficulties difficultyType;

    public ChallengeNode(Stats challenge, ChallengeDifficulties type)
    {
        challengeStats = challenge;
        difficultyType = type;
    }

    protected override void Start()
    {
        combinedPartyStats = new Stats(0,0,0,0,0);
        for (int i = 0; i < _currentState.adventurers.Count; i++)
        {
            combinedPartyStats.Add(_currentState.adventurers[i].Char_Stats);
        }
    }

    private bool MeetAllChallenges(int bonus)
    {
        bool combatMet = combinedPartyStats.Combat.Value + Random.Range(1, bonus) >= challengeStats.Combat.Value;
        bool arcaneMet = combinedPartyStats.Arcane.Value + Random.Range(1, bonus) >= challengeStats.Arcane.Value;
        bool socialMet = combinedPartyStats.Social.Value + Random.Range(1, bonus) >= challengeStats.Social.Value;
        bool dungeonMet = combinedPartyStats.Dungeoneering.Value + Random.Range(1, bonus) >= challengeStats.Dungeoneering.Value;
        bool natureMet = combinedPartyStats.Nature.Value + Random.Range(1, bonus) >= challengeStats.Nature.Value;

        return combatMet & arcaneMet;
    }

    private int ChallengeToBonusMap()
    {
        switch (difficultyType)
        {
            case ChallengeDifficulties.Heroic:
                return 1;
            case ChallengeDifficulties.Expert:
                return 10;
            case ChallengeDifficulties.Veteran:
                return 20;
            case ChallengeDifficulties.Adept:
                return 40;
            case ChallengeDifficulties.Novice:
                return 80;
            default:
                return 100;
        }
    }

    public override void Decide(bool value)
    {
        if (!value)
        {
            // Back down
            Decision = Option2;
            return;
        }
        int difficultyBonus = ChallengeToBonusMap();
        if (MeetAllChallenges(difficultyBonus))
        {
            Decision = Option1;
            return;
        }

        Decision = Option2;
    }

    private float CalculateStatSuccessLikelihood(int challengeStatValue, int partyStatValue, int bonus)
    {
        float numerator = Mathf.Clamp01(challengeStatValue - partyStatValue);
        float denominator = bonus;
        return 1 - (numerator / denominator);
    }

    public float ShowSuccessLikelihood()
    {
        int difficultyBonus = ChallengeToBonusMap();
        Stats cs = challengeStats;
        Stats ps = combinedPartyStats;
        float combatChallenge = CalculateStatSuccessLikelihood(cs.Combat.Value, ps.Combat.Value, difficultyBonus);
        float socialChallenge = CalculateStatSuccessLikelihood(cs.Social.Value, ps.Social.Value, difficultyBonus);
        float natureChallenge = CalculateStatSuccessLikelihood(cs.Nature.Value, ps.Nature.Value, difficultyBonus);
        float arcaneChallenge = CalculateStatSuccessLikelihood(cs.Arcane.Value, ps.Arcane.Value, difficultyBonus);
        float dungeoneeringChallenge = CalculateStatSuccessLikelihood(cs.Dungeoneering.Value, ps.Dungeoneering.Value, difficultyBonus);

        return combatChallenge * socialChallenge * natureChallenge * arcaneChallenge * dungeoneeringChallenge;
        
    }

    protected override void End()
    {
        throw new System.NotImplementedException();
    }
}
