using System.Collections.Generic;
using UnityEngine;

public class AdventurerRecruiter{

    private int _baseSearchTimer;
    private int _currentSearchTimer;
    private float _recruitmentPower;
    private bool _currentlySearching;
    private int _currentSearch;

    private int _searchPowerModifier;

    private List<Adventurer> _currentResults;
    
    const int BASE_RESULTS = 3;

    public AdventurerRecruiter(){
        _baseSearchTimer = 1000;
        _currentSearchTimer = _baseSearchTimer;
        _currentSearch = 0;
        _searchPowerModifier = 0;
        _currentlySearching = false;
        _currentResults = new List<Adventurer>();
        TimeManager.Tick += Tick;
    }

    public void RecruitmentCall(int budget){
        _currentlySearching = true;
        int budgetModifier = budget / 1000;
        _searchPowerModifier = budgetModifier;
        float first_coef = -1*budgetModifier*0.3f;
        float second_coef = -1*budgetModifier*0.5f;
        _currentSearchTimer = (int)( _currentSearchTimer*(Mathf.Exp(first_coef) + Mathf.Exp(second_coef)));
    }

    public void SearchIsOver(){
        ProduceResults();
        _currentSearch = 0;
        _currentlySearching = false;
        _currentSearchTimer = _baseSearchTimer;
        _searchPowerModifier = 0;
    }

    public void ProduceResults(){
        int results_to_generate = BASE_RESULTS + _searchPowerModifier;
        _currentResults = AdventurerFactory.MakeAdventurerList(results_to_generate);

        foreach(Adventurer adv in _currentResults){
            Debug.Log(adv.ToString());
        }
    }

    public bool ResultsExist(){
        return _currentResults.Count > 0;
    }

    public void Tick(){
        if (_currentlySearching){
            _currentSearch++;

            if ( _currentSearch >= _currentSearchTimer){
                SearchIsOver();
            }
        }
    }

    public Adventurer SelectResult(int index){
        Adventurer adv = _currentResults[index];
        _currentResults.Clear();
        return adv;
    }
}