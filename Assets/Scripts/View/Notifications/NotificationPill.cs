
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

    private bool isShowing;
    private void Start()
    {
        TaskNotifyQueue.newEvent += NotificationResponse;
        isShowing = false;
    }

    public void ShowNotificationData()
    {
        TaskNotify taskNotify = TaskNotifyQueue.ViewTaskNotify();
        panel.SetNotificationData(taskNotify);
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
        isShowing = true;
        animator.SetBool("bShow", isShowing);
    }

    private void OnDestroy()
    {
        TaskNotifyQueue.newEvent -= NotificationResponse;
    }

    public void NotificationResponse()
    {
        if (isShowing)
        {
            UpdateNotification();
        } else
        {
            Show();
        }
    }


    public override void Dismiss()
    {
        TryDismiss();
    }

    public void TryDismiss()
    {
        if (TaskNotifyQueue.Count > 0) {
            childAnimator.SetBool("bOpen", false);
            UpdateNotification();
        } else
        {
            isShowing = false;
            animator.SetBool("bShow", isShowing);
        }
    }
}