using System;

public class Stat {
    public int Value {get; set;}
    public string Name {get; private set;}

    public Stat(int value, string name){
        this.Value = value;
        this.Name = name;
    }

    public void Add(int value){
        this.Value = this.Value + value;
    }
}

public enum StatName{
    Combat,
    Dungeoneering,
    Nature,
    Social,
    Arcane
}


public class Stats{
    public Stat Combat {get; private set;}
    public Stat Dungeoneering {get; private set;}
    public Stat Nature {get; private set;}

    public Stat Social {get; private set;}

    public Stat Arcane {get; private set;}

    public Stats(int combat, int dungeoneering, int nature, int social, int arcane){
        Combat = new Stat(combat, "Combat");
        Dungeoneering = new Stat(dungeoneering, "Dungeoneering");
        Nature = new Stat(nature, "Nature");
        Social = new Stat(social, "Social");
        Arcane = new Stat(arcane, "Arcane");
    }

    public void Add(Stats other){
        Combat.Add(other.Combat.Value);
        Dungeoneering.Add(other.Dungeoneering.Value);
        Nature.Add(other.Nature.Value);
        Social.Add(other.Social.Value);
        Arcane.Add(other.Arcane.Value);
    }

    public int Get(StatName stat)
    {
        switch(stat)
        {
            case StatName.Combat: return Combat.Value;
            case StatName.Dungeoneering: return Dungeoneering.Value;
            case StatName.Nature: return Nature.Value;
            case StatName.Social: return Social.Value;
            default: return Arcane.Value;
        }
    }

    public static float GetMaxValue()
    {
        return 100.0f;
    }

    public override string ToString()
    {
        string s = String.Format("S{0} N{1} A{2} C{3} D{4}", 
            Social.Value, Nature.Value, Arcane.Value, Combat.Value, Dungeoneering.Value);
        return s;
    }
}