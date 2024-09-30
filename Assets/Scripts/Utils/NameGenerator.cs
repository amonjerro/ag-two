using System;

public class NameGenerator{

    private readonly static string[] elfSuffixes = {"el", "il", "ir", "rod","bor","thir","fin","or","lad"};
    private readonly static string[] elfPrefixes = {"Fin", "Ae","Gal","Gwin","Thran","Nim","Oro","Van","Val","Mel","Gil"};
    private readonly static string[] elfMiddleSyllable = {"rod", "lan", "gol","gal","gor", "mil","dan","ri","ril","lun","mun","run","da","fea"};


    private readonly static string[] dwarfPrefixes = {"Bal","Glo","Dûr","Bo","Do","Vin","Han","Brun","Fun","Gun"};
    private readonly static string[] dwarfSuffixes = {"in","ri","fur","ndalfr","nar","din","ldr"};

    private readonly static string[] humanFirstNames = {"James","Arthur","Alfred","Damian","Thomas","Stephen","Edward","Ralph","Andrew","Richard","Bruce","Percival","Mitchell"};
    private readonly static string[] humanLastNames = {"Archer","Smith","Cooper","Cook","Cadderly","Freeman","Snow","Storm","Stone","Green","Black","Browne","Shepard"};

    private readonly static Random _rnd = new Random();

    public static string GenerateName(RaceType race){
        switch(race){
            case (RaceType.Elf):
                return MakeElfName();
            case (RaceType.Dwarf):
                return MakeDwarfName();
            default:
                return MakeHumanName(); 

        }
    }

    private static string MakeDwarfName(){
        int first_name_ix = _rnd.Next(dwarfPrefixes.Length);
        int last_name_ix = _rnd.Next(dwarfSuffixes.Length);

        return dwarfPrefixes[first_name_ix]+dwarfSuffixes[last_name_ix];
    }

    private static string MakeElfName(){
        int first_name_ix = _rnd.Next(elfPrefixes.Length);
        int middle_syllable_ix = _rnd.Next( elfMiddleSyllable.Length);
        int last_name_ix = _rnd.Next(elfSuffixes.Length);

        if (elfSuffixes[last_name_ix] == elfMiddleSyllable[middle_syllable_ix]){
             return elfPrefixes[first_name_ix]+" "+elfSuffixes[last_name_ix];
        }

        return elfPrefixes[first_name_ix]+elfMiddleSyllable[middle_syllable_ix]+elfSuffixes[last_name_ix];
       
    }

    private static string MakeHumanName(){
        int first_name_ix = _rnd.Next(humanFirstNames.Length);
        int last_name_ix = _rnd.Next(humanLastNames.Length);

        return humanFirstNames[first_name_ix]+" "+humanLastNames[last_name_ix];
    }
}