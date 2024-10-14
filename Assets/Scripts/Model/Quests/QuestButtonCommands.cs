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
    RosterWidget roster;
    UIPanel dismissPanel;
    RectTransform buttonLocation;
    Vector3 offset;
    int index;

    public ShowRosterCommand(RectTransform transform, UIPanel dismiss, RosterWidget roster, Vector3 offset, int index)
    {
        buttonLocation = transform;
        this.roster = roster;
        this.offset = offset;
        dismissPanel = dismiss;
        this.index = index;
    }

    public override void Execute()
    {

        RectTransform rosterTransform = roster.GetComponent<RectTransform>();
        Vector3 targetLocation = Vector3.zero;
        Quaternion targetRotation = Quaternion.identity;
        buttonLocation.GetLocalPositionAndRotation(out targetLocation, out targetRotation);
        rosterTransform.SetLocalPositionAndRotation(new Vector3(targetLocation.x + offset.x, targetLocation.y,0), targetRotation);
        roster.SetBoundButton(index, buttonLocation.gameObject.GetComponent<QuestButton>());
        roster.Show();
        
        dismissPanel.Show();
    }
}

public class DismissPanelCommand : AQuestCommand
{
    UIPanel dismissable;
    public DismissPanelCommand(UIPanel panel)
    {
        dismissable = panel;
    }

    public override void Execute()
    {
        dismissable.Dismiss();
    }
}

public class EmbarkCommand : DismissPanelCommand
{
    AdventurerManager man;
    QuestData data;
    public EmbarkCommand(AdventurerManager manager, QuestData data, UIPanel dismissable) : base(dismissable)
    {
        man = manager;
        this.data = data;
    }


    public override void Execute() {
        man.BindToQuest(data);
        base.Execute();
    }
}