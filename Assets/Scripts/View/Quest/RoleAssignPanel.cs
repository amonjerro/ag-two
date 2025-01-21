using System.Collections.Generic;
using UnityEngine;

public class RoleAssignPanel : UIPanel
{

    [SerializeField]
    Vector3 rosterWidgetOffset;

    [SerializeField]
    CommandButton embarkButton;

    StatBlock[] statBlocks;
    AdventurerManager managerReference;

    private void Start()
    {
        SetManagerReference();
        statBlocks = GetComponentsInChildren<StatBlock>();
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

}