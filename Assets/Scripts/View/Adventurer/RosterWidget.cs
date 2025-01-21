using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RosterWidget : UIPanel
{
    [SerializeField]
    private TextMeshProUGUI adventurerName;

    [SerializeField]
    private TextMeshProUGUI adventurerLevel;

    StatBlock[] statBlocks;

    Animator animator;
    AdventurerManager manager;
    int currentIndex;
    int rosterIndex;
    List<Adventurer> _availableAdventurers;
    bool bSetUp = false;

    private void OnEnable()
    {
        if (bSetUp)
        {
            return;
        }
        manager = ServiceLocator.Instance.GetService<AdventurerManager>();
        statBlocks = GetComponentsInChildren<StatBlock>();
        animator = GetComponent<Animator>();
        bSetUp = true;

    }


    private void ShowAdventurer()
    {
        Adventurer adventurer = _availableAdventurers[currentIndex];

        foreach(StatBlock sb in statBlocks)
        {
            sb.UpdateStatSliderValue(adventurer);
        }

        // Text Updates
        adventurerName.text = adventurer.Name;
        adventurerLevel.text = $"Lv. {adventurer.Level}";
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
        ShowAdventurer();
    }

    public void PreviousAdventurer()
    {
        currentIndex--;
        if (currentIndex == -1)
        {
            currentIndex = _availableAdventurers.Count - 1;
        }
        ShowAdventurer();
    }

    public void SetRosterIndex(int i)
    {
        rosterIndex = i;
    }

    public override void Show()
    {
        if (manager == null)
        {
            manager = ServiceLocator.Instance.GetService<AdventurerManager>();
        }
        currentIndex = 0;
        _availableAdventurers = manager.GetAvailableAdventurers();
        ShowAdventurer();
        animator.SetBool("bShow", true);
    }



    public override void Dismiss()
    {
        foreach (StatBlock sb in statBlocks) {
            sb.Reset();
        }
        animator.SetBool("bShow", false);
    }

    private void SetAsInactive()
    {
        gameObject.SetActive(false);
    }
}