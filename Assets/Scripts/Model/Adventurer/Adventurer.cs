using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventurer
{
    public string Name {get; set;}
    public int Level {get; private set;}
    private int xp;

    private bool _onMission;

    public Stats Char_Stats{get; private set;}
    public string race;

    public Adventurer(){

        Char_Stats = new Stats(0,0,0,0,0);
        Level = 1;
        _onMission = false;
        xp = 0;
    }

    public Adventurer(string name, RaceType race){
        Char_Stats = new Stats(0,0,0,0,0);
        Char_Stats.Add(RaceFactory.GetRaceStats(race));
        this.race = RaceFactory.GetRaceName(race);
        Level = 1;
        _onMission = false;
        xp = 0;
        Name = name;
    }

    public void AssignXp(int xp){
        this.xp += xp;
        if (LevellingController.ShouldLevelUp(this.xp, this.Level)){
            //Perform level up operation
            this.Level++;
            AdjustSkills();
        }
    }

    public void LevelAdjust(int level){
        for (int i = 0; i < (level - this.Level); i++ ){
            AdjustSkills();
        }
        this.Level = level;
    }

    public void AdjustSkills(){
        //TO DO
    }


    public bool IsOnMission(){
        return _onMission;
    }

    public void SendOnMission(){
        _onMission = true;
    }

    public void ReturnFromMission()
    {
        _onMission = false;
    }

    public override string ToString(){
        return "Name: "+this.Name+" Race: "+this.race;
    }

}
