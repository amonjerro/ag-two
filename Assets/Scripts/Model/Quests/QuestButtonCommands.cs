using UnityEngine;

public abstract class AQuestCommand
{
    public abstract void Execute();
}

public class AbandonQuestCommand : AQuestCommand
{
    QuestController questController;
    QuestNodePanel questUIManager;
    public AbandonQuestCommand(QuestController controller, QuestNodePanel uiManager) { 
        questController = controller;
        questUIManager = uiManager;
    }

    public override void Execute()
    {
        questController.StopActiveQuest(questUIManager.GetOpenNode());
        questUIManager.Dismiss();
    }
}

public class DecisionAcceptCommand : AQuestCommand {

    QuestController questController;
    QuestNodePanel questUIManager;
    public DecisionAcceptCommand(QuestController controller, QuestNodePanel uIManager)
    {
        questController = controller;
        questUIManager = uIManager;
    }

    public override void Execute()
    {
        questController.DecideNode(questUIManager.GetOpenNode(), true);
        questUIManager.Dismiss();
    }
}

public class DecisionRejectCommand : AQuestCommand
{
    QuestController questController;
    QuestNodePanel questUIManager;
    public DecisionRejectCommand(QuestController controller, QuestNodePanel uIManager)
    {
        questController = controller;
        questUIManager = uIManager;
    }

    public override void Execute()
    {
        questController.DecideNode(questUIManager.GetOpenNode(), false);
        questUIManager.Dismiss();
    }
}


public class NextPageCommand : AQuestCommand
{
    QuestNodePanel questUIManager;
    public NextPageCommand(QuestNodePanel uiManager)
    {
        questUIManager = uiManager;
    }

    public override void Execute()
    {
        questUIManager.CyclePage();
    }
}

public class AcknowledgeCommand : AQuestCommand
{
    QuestNodePanel questUIManager;
    QuestController questController;

    public AcknowledgeCommand(QuestController questController, QuestNodePanel questUIManager)
    {
        this.questUIManager = questUIManager;
        this.questController = questController;
    }

    public override void Execute()
    {
        questController.ProceedToNext(questUIManager.GetOpenNode());
        questUIManager.Dismiss();
    }
}

public class SeeOpenQuestCommand : AQuestCommand
{
    QuestData data;
    QuestUIPanel uiPanel;

    public SeeOpenQuestCommand(QuestData data, QuestUIPanel uiPanel)
    {
        this.data = data;
        this.uiPanel = uiPanel;
    }

    public override void Execute()
    {
        uiPanel.SetQuestNode(data.rootNode);
    }
}

public class ShowRosterCommand : AQuestCommand
{
    QuestUIPanel roster;
    RectTransform buttonLocation;
    Vector3 offset;

    public ShowRosterCommand(RectTransform transform, QuestUIPanel roster, Vector3 offset)
    {
        buttonLocation = transform;
        this.roster = roster;
        this.offset = offset;
    }

    public override void Execute()
    {
        RectTransform rosterTransform = roster.GetComponent<RectTransform>();
        Vector3 targetLocation = Vector3.zero;
        Quaternion targetRotation = Quaternion.identity;
        buttonLocation.GetPositionAndRotation(out targetLocation, out targetRotation);
        rosterTransform.SetPositionAndRotation(targetLocation + offset, targetRotation);

        roster.Show();
    }
}