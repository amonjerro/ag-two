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

public class SeeQuestNode : AQuestCommand
{
    AQuestNode data;
    QuestUIPanel questPanel;

    public SeeQuestNode(AQuestNode data, QuestUIPanel questPanel)
    {
        this.data = data;
        this.questPanel = questPanel;
    }

    public override void Execute()
    {
        questPanel.SetQuestNode(data);
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
    AQuestNode node;
    public EmbarkCommand(AdventurerManager manager, AQuestNode node, UIPanel dismissable) : base(dismissable)
    {
        man = manager;
        data = manager.GetQuestController().GetQuestByKey(node.QuestKey);
        this.node = node;
    }


    public override void Execute() {
        man.BindToQuest(data);
        man.GetQuestController().AddActiveQuest(node.Next);
        base.Execute();
    }
}