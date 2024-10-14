using TMPro;
using UnityEngine;

public class RosterWidget : UIPanel
{
    [SerializeField]
    GameObject pickAdventurerPrefab;

    QuestButton boundButton;
    int boundIndex;
    RoleAssignPanel buttonParentPanel;

    AdventurerManager manager;


    [SerializeField]
    GameObject contentDrawer;

    private void Start()
    {
        manager = ServiceLocator.Instance.GetService<AdventurerManager>();
    }

    private void DestroyOpenQuestObjects()
    {
        foreach (Transform child in contentDrawer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void CreateRosterButtons()
    {
        foreach(Adventurer adventurer in manager.GetAvailableAdventurers())
        {
            GameObject button = Instantiate(pickAdventurerPrefab, contentDrawer.transform);
            PickAdventurerButton buttonLogic = button.GetComponent<PickAdventurerButton>();
            buttonLogic.SetCommand(new StageAdventurerCommand(
                boundButton,boundIndex,
                buttonParentPanel,
                adventurer, manager, this)
            );
            TextMeshProUGUI textGUI = button.GetComponentInChildren<TextMeshProUGUI>();
            textGUI.text = adventurer.Name;
        }
    }

    public override void Show()
    {
        // Destroy pre-existing quest objects in list
        DestroyOpenQuestObjects();

        // Create the roster objects
        CreateRosterButtons();

    }

    public void SetBoundButton(int index, QuestButton button)
    {
        boundIndex = index;
        boundButton = button;
        buttonParentPanel = boundButton.GetComponentInParent<RoleAssignPanel>();
    }

    public override void Dismiss()
    {
        GetComponentInParent<UIDismissTool>().Dismiss();
    }
}