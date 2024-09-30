using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdventurerProfile : UIPanel
{
    private Animator animationController;
    public List<StatBlock> statBlocks = new List<StatBlock>();
    [SerializeField]
    TextMeshProUGUI adventurerName;

    private void Start()
    {
        animationController = GetComponent<Animator>();
    }


    public override void Show()
    {
        animationController.SetBool("bShow", true);
    }

    public override void Dismiss()
    {
        animationController.SetBool("bShow", false);
    }

    public void GetData(Adventurer adventurer)
    {
        adventurerName.text = adventurer.Name;
        foreach (StatBlock statBlock in statBlocks) { 
            statBlock.UpdateStatSliderValue(adventurer);
        }
        Show();
    }
}
