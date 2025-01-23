using SaveGame;
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
    
    // Start is called before the first frame update
    void Awake()
    {
        _availableAdventurers = new HashSet<Adventurer>();
        adventurerRoster = new List<Adventurer>();
        _adventurerRecruiter = new AdventurerRecruiter();
        _questController = new QuestController(_questData);
        _stagingRoster = new Dictionary<int, Adventurer>();

        DebugSetup();
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    private void DebugSetup()
    {
        // If we have data saved in the GameInstance, then we're good to get started.
        if (GameInstance.roster.Count > 0)
        {
            adventurerRoster = GameInstance.roster;
            return;
        }


        // Otherwise, let's instantiate some random ass adventurers to put in the list.
        adventurerRoster = AdventurerFactory.MakeAdventurerList(3);
        for (int i = 0; i < adventurerRoster.Count; i++)
        {
            _availableAdventurers.Add(adventurerRoster[i]);
            GameInstance.roster.Add(adventurerRoster[i]);
        }
    }

    public void AddAdventurer(Adventurer item){
        adventurerRoster.Add(item);
    }

    public void RemoveAdventurer(Adventurer item){
        adventurerRoster.Remove(item);
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
            _availableAdventurers.Add(_stagingRoster[index]);
            _stagingRoster[index] = adventurer;
            _availableAdventurers.Remove(adventurer);
        } else
        {
            _stagingRoster.Add(index, adventurer);
            _availableAdventurers.Remove(adventurer);
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

    public int AvailabilityCount() {
        return _availableAdventurers.Count;
    }

    public void MakeUnavailable(Adventurer adventurer)
    {
        _availableAdventurers.Remove(adventurer);
    }

    public void MakeAvailable(Adventurer adventurer)
    {
        _availableAdventurers.Add(adventurer);
    }
}
