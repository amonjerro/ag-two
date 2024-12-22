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
    CommandButton embarkButton;

    [SerializeField]
    CommandButton cancel;

    StatBlock[] statBlocks;
    AdventurerManager managerReference;
    QuestData questData;
    AQuestNode questNode;

    private void Start()
    {
        SetManagerReference();
        statBlocks = GetComponentsInChildren<StatBlock>();

        // Instantiate the button commands
        PickAdventurerButton[] buttons = GetComponentsInChildren<PickAdventurerButton>();
        for(int i = 0; i < buttons.Length; i++) {
            PickAdventurerButton button = buttons[i];
            button.SetCommand(
                new ShowRosterCommand(
                    button.GetComponent<RectTransform>(),
                    widgetDismiss,
                    rosterWidget, 
                    rosterWidgetOffset, i
                )
            );
        }
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
        questNode = node;
        //embarkButton.SetCommand(new EmbarkCommand(managerReference, questNode, this));
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