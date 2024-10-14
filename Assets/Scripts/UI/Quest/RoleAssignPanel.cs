using System.Collections.Generic;
using UnityEngine;

public class RoleAssignPanel : QuestUIPanel
{
    [SerializeField]
    RosterWidget rosterWidget;

    [SerializeField]
    UIPanel widgetDismiss;

    [SerializeField]
    Vector3 rosterWidgetOffset;

    [SerializeField]
    QuestButton embarkButton;

    [SerializeField]
    QuestButton cancel;

    StatBlock[] statBlocks;
    AdventurerManager managerReference;
    QuestData questData;

    private void Start()
    {
        SetManagerReference();
        statBlocks = GetComponentsInChildren<StatBlock>();

        // Instantiate the button commands
        QuestButton[] buttons = GetComponentsInChildren<QuestButton>();
        for(int i = 0; i < buttons.Length; i++) {
            QuestButton button = buttons[i];
            button.SetCommand(
                new ShowRosterCommand(
                    button.GetComponent<RectTransform>(),
                    widgetDismiss,
                    rosterWidget, 
                    rosterWidgetOffset, i
                )
            );
        }
        cancel.SetCommand(new DismissPanelCommand(this));
        embarkButton.SetCommand(new EmbarkCommand(managerReference, questData, this));

    }

    public override void Dismiss()
    {
        GetComponentInParent<QuestEventUI>().Dismiss();
    }

    public override void Show()
    {
        managerReference.ResetStaging();
    }

    private void SetManagerReference()
    {
        if (managerReference == null)
        {
            managerReference = ServiceLocator.Instance.GetService<AdventurerManager>();
        }
    }

    public override void SetQuestNode(AQuestNode node)
    {
        SetManagerReference();
        questData = managerReference.GetQuestController().GetQuestByKey(node.QuestKey);
    }

    public void UpdateStats(Dictionary<int, Adventurer> stagingRoster)
    {
        int value;
        for(int i = 0; i < statBlocks.Length; i++)
        {
            StatBlock statBlock = statBlocks[i];
            value = 0;
            foreach(KeyValuePair<int, Adventurer> kvp in stagingRoster)
            {
                value += kvp.Value.Char_Stats.Get(statBlock.statType);
            }
            statBlock.SetSliderValue(value / Stats.GetMaxValue());
        }
    }

    public void ResetStats()
    {
        for (int i = 0; i < statBlocks.Length; i++)
        {
            statBlocks[i].SetSliderValue(0);
        }
    }

}