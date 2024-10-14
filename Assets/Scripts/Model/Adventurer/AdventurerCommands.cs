using UnityEngine;
using UnityEngine.UI;

public abstract class AAdventurerCommand
{
    public abstract void Execute();
}

public class StageAdventurerCommand : AAdventurerCommand
{
    QuestButton button;
    int boundIndex;
    Adventurer adventurer;
    RoleAssignPanel roleAssignPanel;
    AdventurerManager man;
    RosterWidget rosterWidget;
    public StageAdventurerCommand(QuestButton qb, int index, RoleAssignPanel panel, Adventurer adv, AdventurerManager advMan, RosterWidget widget) { 
        button = qb;
        boundIndex = index;
        roleAssignPanel = panel;
        adventurer = adv;
        man = advMan;
        rosterWidget = widget;
    }

    public override void Execute()
    {

        Image image = button.GetComponentInChildren<Image>();
        if ( image != null )
        {
            // Set the image
        }
        // Handle Adventurer Selection
        Debug.Log(boundIndex);
        man.AddToStagingRoster(boundIndex, adventurer);
        man.MakeUnavailable(adventurer);

        // Update UI
        roleAssignPanel.UpdateStats(man.GetStagingRoster());
        rosterWidget.Dismiss();
    }
}