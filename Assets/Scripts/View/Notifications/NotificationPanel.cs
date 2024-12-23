using System.Collections.Generic;
using Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class NotificationPanel : UIPanel
{

    List<AbstractCommand> commands = new List<AbstractCommand>();
    List<string> commandNames = new List<string>();

    [SerializeField]
    UIPanel sidePanel;

    [SerializeField]
    Image image;
    
    [SerializeField]
    TextMeshProUGUI notificationTitle;

    [SerializeField]
    TextMeshProUGUI notificationText;

    [SerializeField]
    VerticalButtonContainer verticalButtonContainer;

    [SerializeField]
    Animator animator;

    public void AddCommand(string commandString, AbstractCommand command)
    {
        commands.Add(command);
        commandNames.Add(commandString);
    }

    private void PopulateCommands(Task task, TaskNotify notification)
    {
        switch (task.TaskType) {
            case TaskType.Quest:
                QuestTask questTask = (QuestTask)task;
                AQuestNode questNode = questTask.questNode;
                switch (questNode.NodeType)
                {
                    case NodeTypes.Decision:
                    case NodeTypes.Challenge:
                        DecisionNode decNode = (DecisionNode)questNode;
                        AddCommand(decNode.Option1String, new DecisionAcceptCommand(decNode, (QuestNodeNotify) notification));
                        AddCommand(decNode.Option2String, new DecisionRejectCommand(decNode, (QuestNodeNotify) notification));
                        break;
                    default:
                        break;
                }
                break;
            case TaskType.Build:
                AddCommand("Carry On", new BuildAcknowledge());
                break;
            default:
                return;
        }
    }

    public void SetNotificationData(TaskNotify notify)
    {
        Task task = notify.task;
        notificationTitle.text = task.Title;
        notificationText.text = task.LongDescription;
        PopulateCommands(task, notify);
    }

    public override void Show()
    {   
        for (int i = 0; i < commands.Count; i++) {
            verticalButtonContainer.AddChild(commandNames[i], commands[i]);
        }
        
        animator.SetBool("bOpen", true);   
    }

    public override void Dismiss()
    {
        commands.Clear();
        commandNames.Clear();
        animator.SetBool("bOpen", false);
    }
}