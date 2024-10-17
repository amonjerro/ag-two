using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AdventurerManager : MonoBehaviour
{
    public GameObject adventurerPrefab;
    public List<Adventurer> adventurerRoster;
    private AdventurerRecruiter _adventurerRecruiter;
    private Dictionary<int, Adventurer> _stagingRoster;
    private HashSet<Adventurer> _availableAdventurers;


    private QuestController _questController;
    [SerializeField]
    UIPanel questMessageQueuePanel;

    [SerializeField]
    List<SO_QuestData> _questData;
    
    public AdventurerProfile profile;
    
    // Start is called before the first frame update
    void Awake()
    {
        _availableAdventurers = new HashSet<Adventurer>();
        adventurerRoster = new List<Adventurer>();
        _adventurerRecruiter = new AdventurerRecruiter();
        _questController = new QuestController(_questData, (NotificationPill)questMessageQueuePanel);
        _stagingRoster = new Dictionary<int, Adventurer>();

        DebugSetup();
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    private void DebugSetup()
    {
        int xLimit = 5;
        int yLimit = 3;
        adventurerRoster = AdventurerFactory.MakeAdventurerList(3);
        for (int i = 0; i < adventurerRoster.Count; i++)
        {
            Vector2 randomStartingPosition = new Vector2(Random.Range(-xLimit, xLimit), Random.Range(-yLimit, yLimit));
            GameObject adventurer = Instantiate(adventurerPrefab, randomStartingPosition, Quaternion.identity);
            AdventurerNPC npcInformation = adventurer.GetComponent<AdventurerNPC>();
            npcInformation.index = i;
            _availableAdventurers.Add(adventurerRoster[i]);
        }
    }

    public void AddAdventurer(Adventurer item){
        adventurerRoster.Add(item);
    }

    public void RemoveAdventurer(Adventurer item){
        adventurerRoster.Remove(item);
    }

    public void ShowAdventurerProfile(NPCInformation information)
    {
        profile.GetData(adventurerRoster[information.index]);
    }

    public List<QuestData> GetOpenQuests()
    {
        return _questController.GetOpenQuests();
    }

    public QuestController GetQuestController()
    {
        return _questController;
    }

    public void AddToStagingRoster(int index, Adventurer adventurer)
    {
        if (_stagingRoster.ContainsKey(index))
        {
            _stagingRoster[index] = adventurer;
        } else
        {
            _stagingRoster.Add(index, adventurer);
        }
    }

    public Dictionary<int, Adventurer> GetStagingRoster()
    {
        return _stagingRoster;
    }

    public void ResetStaging()
    {
        _stagingRoster.Clear();
    }

    public List<Adventurer> GetAvailableAdventurers()
    {
        return _availableAdventurers.ToList();
    }

    public void MakeUnavailable(Adventurer adventurer)
    {
        _availableAdventurers.Remove(adventurer);
    }

    public void MakeAvailable(Adventurer adventurer)
    {
        _availableAdventurers.Add(adventurer);
    }

    public void BindToQuest(QuestData questData)
    {
        _questController.BindStagingToQuest(questData, _stagingRoster);
    }
}
