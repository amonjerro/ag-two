
using GameCursor;
using UnityEngine;

public class NotificationPill : UIPanel
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    Animator childAnimator;

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

        childAnimator.SetBool("bOpen", true);

        ServiceLocator.Instance.GetService<CursorManager>().SetCursorState(CursorStates.QuestEvent);

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
        TryDismiss();
    }

    public void TryDismiss()
    {
        if (questController.NotificationsEmpty)
        {
            animator.SetBool("bShow", false);
        }
    }
}