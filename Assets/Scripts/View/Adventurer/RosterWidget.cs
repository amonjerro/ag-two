using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RosterWidget : UIPanel
{
    Button button;
    AdventurerManager manager;
    int currentIndex;
    int rosterIndex;
    List<Adventurer> _availableAdventurers;

    private void Start()
    {
        manager = ServiceLocator.Instance.GetService<AdventurerManager>();
    }


    private void ShowAdventurer()
    {
        Adventurer adventurer = _availableAdventurers[currentIndex];

    }

    public void StageAdventurer()
    {
        manager.AddToStagingRoster(rosterIndex, _availableAdventurers[currentIndex]);
    }

    public void NextAdventurer()
    {
        currentIndex++;
        if(currentIndex == _availableAdventurers.Count)
        {
            currentIndex = 0;
        }

    }

    public void PreviousAdventurer()
    {
        currentIndex--;
        if (currentIndex == -1)
        {
            currentIndex = _availableAdventurers.Count - 1;
        }
    }

    public void SetRosterIndex(int i)
    {
        rosterIndex = i;
    }

    public override void Show()
    {
        currentIndex = 0;
        ShowAdventurer();
    }

    public override void Dismiss()
    {

    }
}