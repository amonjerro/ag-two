using System;
using System.Collections.Generic;

public static class AdventurerFactory 
{
    /// <summary>
    /// Populates a random list of adventurers
    /// </summary>
    /// <param name="count">How many adventurers to create</param>
    /// <returns>A list of adventurer objects</returns>
    public static List<Adventurer> MakeAdventurerList(int count){
        List<Adventurer> list = new List<Adventurer>();
        for (int i = 0; i < count; i++){
            list.Add(MakeAdventurer());
        }
        return list;
    }

    public static List<Adventurer> MakeAdventurerList(int count, int levelModifier){
        List<Adventurer> list = new List<Adventurer>();
        for (int i = 0; i < count; i++){
            list.Add(MakeAdventurer(levelModifier));
        }
        return list;
    }

    public static Adventurer MakeAdventurer(){
        switch(RandomUtils.RandomEnumValue<SpeciesType>()){
            case (SpeciesType.Human):
                return MakeHuman(1);
            case (SpeciesType.Kedi):
                return MakeKedi(1);
            case (SpeciesType.Corvo):
                return MakeCorvo(1);
            default:
                return MakeHuman(1);
        }
    }

    public static Adventurer MakeAdventurer(int level){
        switch(RandomUtils.RandomEnumValue<SpeciesType>()){
            case (SpeciesType.Human):
                return MakeHuman(level);
            case (SpeciesType.Kedi):
                return MakeKedi(level);
            case (SpeciesType.Corvo):
                return MakeCorvo(level);
            default:
                return MakeHuman(level);
        }
    }

    private static Adventurer MakeKedi(int level){
        Adventurer adv = new Adventurer(NameGenerator.GenerateName(SpeciesType.Kedi), SpeciesType.Kedi);
        if (level > 1){
            adv.LevelAdjust(level);
        }
        return adv;
    }

    private static Adventurer MakeCorvo(int level){
        Adventurer adv = new Adventurer(NameGenerator.GenerateName(SpeciesType.Corvo), SpeciesType.Corvo);
        if (level > 1){
            adv.LevelAdjust(level);
        }
        return adv;
    }

    private static Adventurer MakeHuman(int level){
        Adventurer adv = new Adventurer(NameGenerator.GenerateName(SpeciesType.Human), SpeciesType.Human);
        if (level > 1){
            adv.LevelAdjust(level);
        }
        return adv;
    }

}