using Tasks;
public class AbandonQuestCommand : AbstractCommand
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

public class DecisionAcceptCommand : AbstractCommand {

    DecisionNode internalNodeReference;
    QuestNodeNotify notification;
    public DecisionAcceptCommand(DecisionNode decisionNode, QuestNodeNotify notification)
    {
        internalNodeReference = decisionNode;
        this.notification = notification;
        this.notification.SetQuestNode(decisionNode);
    }

    public override void Execute()
    {
        internalNodeReference.Decide(true);
        notification.Acknowledge();
    }
}

public class DecisionRejectCommand : AbstractCommand
{

    DecisionNode internalNodeReference;
    QuestNodeNotify notification;
    public DecisionRejectCommand(DecisionNode decNode, QuestNodeNotify notification)
    {
        internalNodeReference = decNode;
        this.notification = notification;
        this.notification.SetQuestNode(decNode);
    }

    public override void Execute()
    {
        internalNodeReference.Decide(false);
        notification.Acknowledge();
    }
}


public class AcknowledgeCommand : AbstractCommand
{

    QuestNodeNotify notify;

    public AcknowledgeCommand(QuestNodeNotify notification)
    {
        notify = notification;
    }

    public override void Execute()
    {
        notify.Acknowledge();
    }
}

public class SeeQuestCommand : AbstractCommand
{
    public SeeQuestCommand(QuestData data)
    {

    }

    public override void Execute()
    {

    }
}

public class EmbarkCommand : AbstractCommand
{
    AdventurerManager man;
    QuestData data;
    AQuestNode node;
    public EmbarkCommand() 
    {
    }


    public override void Execute() {

    }
}