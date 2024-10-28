using UnityEngine;

public class QuestEventUI : QuestUIPanel
{
    public UIPanel sidePanel;
    public QuestUIPanel nodeEventPanel;
    public QuestUIPanel roleAssignPanel;

    Animator animator;
    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Show()
    {
        nodeEventPanel.gameObject.SetActive(true);
        nodeEventPanel.Show();
        roleAssignPanel.gameObject.SetActive(false);
        animator.SetBool("bShow", true);
    }

    public override void Dismiss()
    {
        sidePanel.Dismiss();
        ServiceLocator.Instance.GetService<GameCursor.CursorManager>().SetCursorState(GameCursor.CursorStates.FreeHand);
        animator.SetBool("bShow", false);
    }

    public void NextPage()
    {
        nodeEventPanel.gameObject.SetActive(false);
        roleAssignPanel.gameObject.SetActive(true);
    }

    public override void SetQuestNode(AQuestNode data)
    {
        nodeEventPanel.SetQuestNode(data);
        roleAssignPanel.SetQuestNode(data);
    }
}