using System;
using System.Collections.Generic;

public static class AdventurerFactory 
{

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
        switch(RandomUtils.RandomEnumValue<RaceType>()){
            case (RaceType.Human):
                return MakeHuman(1);
            case (RaceType.Elf):
                return MakeElf(1);
            case (RaceType.Dwarf):
                return MakeDwarf(1);
            default:
                return MakeHuman(1);
        }
    }

    public static Adventurer MakeAdventurer(int level){
        switch(RandomUtils.RandomEnumValue<RaceType>()){
            case (RaceType.Human):
                return MakeHuman(level);
            case (RaceType.Elf):
                return MakeElf(level);
            case (RaceType.Dwarf):
                return MakeDwarf(level);
            default:
                return MakeHuman(level);
        }
    }

    private static Adventurer MakeElf(int level){
        Adventurer adv = new Adventurer(NameGenerator.GenerateName(RaceType.Elf), RaceType.Elf);
        if (level > 1){
            adv.LevelAdjust(level);
        }
        return adv;
    }

    private static Adventurer MakeDwarf(int level){
        Adventurer adv = new Adventurer(NameGenerator.GenerateName(RaceType.Dwarf), RaceType.Dwarf);
        if (level > 1){
            adv.LevelAdjust(level);
        }
        return adv;
    }

    private static Adventurer MakeHuman(int level){
        Adventurer adv = new Adventurer(NameGenerator.GenerateName(RaceType.Human), RaceType.Human);
        if (level > 1){
            adv.LevelAdjust(level);
        }
        return adv;
    }

}