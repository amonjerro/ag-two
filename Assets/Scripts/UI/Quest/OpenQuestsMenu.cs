
using System.Collections.Generic;
using UnityEngine;

public class OpenQuestsMenu : UIPanel
{
    [SerializeField]
    QuestNodePanel panel;

    [SerializeField]
    GameObject questOptionPrefab;

    [SerializeField]
    GameObject contentDrawer;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void DestroyOpenQuestObjects()
    {
        foreach(Transform child in contentDrawer.transform)
        {
            Destroy(child);
        }
    }

    private void CreateQuestObjects()
    {
        // Validate no new things have been added since last time?


        // Destroy pre-existing quest objects in list
        DestroyOpenQuestObjects();

        // Get data again
        List<QuestData> questData = ServiceLocator.Instance.GetService<AdventurerManager>().GetOpenQuests();

        // Instantiate
        foreach (QuestData dataItem in questData) {
            GameObject createdObject = Instantiate(questOptionPrefab, contentDrawer.transform);
            AvailableQuest aq = createdObject.GetComponent<AvailableQuest>();
            aq.questData = dataItem;
            aq.panel = panel;
            aq.UpdateSelf();
        }
    }

    public override void Show()
    {
        CreateQuestObjects();
        animator.SetBool("bShow", true);
    }

    public override void Dismiss()
    {
        animator.SetBool("bShow", false);
    }
}
