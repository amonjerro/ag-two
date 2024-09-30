using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerManager : MonoBehaviour
{
    public GameObject adventurerPrefab;
    public List<Adventurer> adventurerRoster;
    private AdventurerRecruiter _adventurerRecruiter;

    private QuestController _questController;
    [SerializeField]
    List<SO_QuestData> _questData;
    
    public AdventurerProfile profile;
    
    // Start is called before the first frame update
    void Start()
    {
        adventurerRoster = new List<Adventurer>();
        _adventurerRecruiter = new AdventurerRecruiter();
        _questController = new QuestController(_questData);
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
        profile.Show();
    }

    public List<QuestData> GetOpenQuests()
    {
        return _questController.GetOpenQuests();
    }
}
