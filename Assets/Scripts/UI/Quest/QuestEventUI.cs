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
        animator.SetBool("bShow", false);
    }

    public void NextPage()
    {
        nodeEventPanel.gameObject.SetActive(false);
        roleAssignPanel.gameObject.SetActive(true);
    }

    public override void SetQuestData(QuestData data)
    {
        nodeEventPanel.SetQuestData(data);
        roleAssignPanel.SetQuestData(data);
    }
}