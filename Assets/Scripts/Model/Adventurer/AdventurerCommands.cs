using UnityEngine;
using UnityEngine.UI;

public abstract class AAdventurerCommand
{
    public abstract void Execute();
}

public class StageAdventurerCommand : AAdventurerCommand
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
        Debug.Log(boundIndex);
        man.AddToStagingRoster(boundIndex, adventurer);
        man.MakeUnavailable(adventurer);

        // Update UI
        roleAssignPanel.UpdateStats(man.GetStagingRoster());
        rosterWidget.Dismiss();
    }
}


public class ShowRosterCommand : AAdventurerCommand
{
    RosterWidget roster;
    UIPanel dismissPanel;
    RectTransform buttonLocation;
    Vector3 offset;
    int index;

    public ShowRosterCommand(RectTransform transform, UIPanel dismiss, RosterWidget roster, Vector3 offset, int index)
    {
        buttonLocation = transform;
        this.roster = roster;
        this.offset = offset;
        dismissPanel = dismiss;
        this.index = index;
    }

    public override void Execute()
    {

        RectTransform rosterTransform = roster.GetComponent<RectTransform>();
        Vector3 targetLocation = Vector3.zero;
        Quaternion targetRotation = Quaternion.identity;
        buttonLocation.GetLocalPositionAndRotation(out targetLocation, out targetRotation);
        rosterTransform.SetLocalPositionAndRotation(new Vector3(targetLocation.x + offset.x, targetLocation.y, 0), targetRotation);
        roster.SetBoundButton(index, buttonLocation.gameObject.GetComponent<PickAdventurerButton>());
        roster.Show();

        dismissPanel.Show();
    }
}