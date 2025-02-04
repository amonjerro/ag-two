using UnityEngine;
using UnityEngine.UI;


public class StageAdventurerCommand : AbstractCommand
{
    PickAdventurerButton button;
    int boundIndex;
    Adventurer adventurer;
    RoleAssignPanel roleAssignPanel;
    AdventurerManager man;
    RosterWidget rosterWidget;
    public StageAdventurerCommand(PickAdventurerButton qb, int index, RoleAssignPanel panel, Adventurer adv, AdventurerManager advMan, RosterWidget widget) { 
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
        man.AddToStagingRoster(boundIndex, adventurer);
        man.MakeUnavailable(adventurer);

        // Update UI
        roleAssignPanel.UpdateStats(man.GetStagingRoster());
        rosterWidget.Dismiss();
    }
}


public class ShowRosterCommand : AbstractCommand
{
    RosterWidget roster;
    UIPanel dismissPanel;

    public ShowRosterCommand(RectTransform transform, RosterWidget roster)
    {
        this.roster = roster;
    }

    public override void Execute()
    {
        roster.Show();
    }
}