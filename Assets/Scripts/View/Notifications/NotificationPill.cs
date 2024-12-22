
using GameCursor;
using Tasks;
using UnityEngine;

public class NotificationPill : UIPanel
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    Animator childAnimator;
    
    [SerializeField]
    NotificationPanel panel;
    private void Start()
    {

    }

    public void ShowNotificationData()
    {
        TaskNotify taskNotify = TaskNotifyQueue.ViewTaskNotify();
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

    }
}