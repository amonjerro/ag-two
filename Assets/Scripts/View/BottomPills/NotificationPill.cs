
using UnityEngine;

public class NotificationPill : UIPanel
{
    [SerializeField]
    Animator animator;
    QuestController questController;
    
    [SerializeField]
    QuestEventUI panel;
    private void Start()
    {
        questController = ServiceLocator.Instance.GetService<AdventurerManager>().GetQuestController();
    }

    public void ShowNotificationData()
    {
        AQuestNode node = questController.GetNotification();
        SeeQuestNode command = new SeeQuestNode(node, panel);
        command.Execute();
        panel.Show();

        if (questController.NotificationsEmpty)
        {
            Dismiss();
        }
    }

    public void UpdateNotification()
    {
        animator.SetTrigger("tBounce");
    }

    public override void Show()
    {
        animator.SetBool("bShow", true);
    }

    public override void Dismiss()
    {
        animator.SetBool("bShow", false);
    }
}