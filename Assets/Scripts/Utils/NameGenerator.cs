using System;

public class NameGenerator{

    private readonly static string[] kediPrefixes = {"Fin", "Ae","Gal","Gwin","Thran","Nim","Oro","Van","Val","Mel","Gil"};
    private readonly static string[] kediMiddleSyllable = {"rod", "lan", "gol","gal","gor", "mil","dan","ri","ril","lun","mun","run","da","fea"};
    private readonly static string[] kediSuffixes = { "el", "il", "ir", "rod", "bor", "thir", "fin", "or", "lad" };

    private readonly static string[] corvoPrefixes = {"Bal","Glo","Dûr","Bo","Do","Vin","Han","Brun","Fun","Gun"};
    private readonly static string[] corvoSuffixes = {"in","ri","fur","ndalfr","nar","din","ldr"};

    private readonly static string[] humanFirstNames = {"James","Arthur","Alfred","Damian","Thomas","Stephen","Edward","Ralph","Andrew","Richard","Bruce","Percival","Mitchell"};
    private readonly static string[] humanLastNames = {"Archer","Smith","Cooper","Cook","Cadderly","Freeman","Snow","Storm","Stone","Green","Black","Browne","Shepard"};

    private readonly static Random _rnd = new Random();

    public static string GenerateName(RaceType race){
        switch(race){
            case (RaceType.Kedi):
                return MakeKediName();
            case (RaceType.Corvo):
                return MakeCorvoName();
            default:
                return MakeHumanName(); 

        }
    }

    private static string MakeCorvoName(){
        int first_name_ix = _rnd.Next(corvoPrefixes.Length);
        int last_name_ix = _rnd.Next(corvoSuffixes.Length);

        return corvoPrefixes[first_name_ix]+corvoSuffixes[last_name_ix];
    }

    private static string MakeKediName(){
        int first_name_ix = _rnd.Next(kediPrefixes.Length);
        int middle_syllable_ix = _rnd.Next( kediMiddleSyllable.Length);
        int last_name_ix = _rnd.Next(kediSuffixes.Length);

        if (kediSuffixes[last_name_ix] == kediMiddleSyllable[middle_syllable_ix]){
             return kediPrefixes[first_name_ix]+" "+kediSuffixes[last_name_ix];
        }

        return kediPrefixes[first_name_ix]+kediMiddleSyllable[middle_syllable_ix]+kediSuffixes[last_name_ix];
       
    }

    private static string MakeHumanName(){
        int first_name_ix = _rnd.Next(humanFirstNames.Length);
        int last_name_ix = _rnd.Next(humanLastNames.Length);

        return humanFirstNames[first_name_ix]+" "+humanLastNames[last_name_ix];
    }
}