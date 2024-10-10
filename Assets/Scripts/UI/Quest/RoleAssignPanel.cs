public class RoleAssignPanel : QuestUIPanel
{
    QuestData data;
    public override void Dismiss()
    {
        GetComponentInParent<UIPanel>().Dismiss();
    }

    public override void Show()
    {
        
    }

    public override void SetQuestNode(AQuestNode node)
    {
        
    }
}