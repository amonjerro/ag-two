public struct Stat {
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


public struct Stats{
    public Stat Combat {get; private set;}
    public Stat Dungeoneering {get; private set;}
    public Stat Nature {get; private set;}

    public Stat Social {get; private set;}

    public Stat Arcane {get; private set;}

    public Stats(int combat, int dungeoneering, int nature, int social, int arcane){
        this.Combat = new Stat(combat, "Combat");
        this.Dungeoneering = new Stat(dungeoneering, "Dungeoneering");
        this.Nature = new Stat(nature, "Nature");
        this.Social = new Stat(social, "Social");
        this.Arcane = new Stat(arcane, "Arcane");
    }

    public void Add(Stats other){
        this.Combat.Add(other.Combat.Value);
        this.Dungeoneering.Add(other.Dungeoneering.Value);
        this.Nature.Add(other.Nature.Value);
        this.Social.Add(other.Social.Value);
        this.Arcane.Add(other.Arcane.Value);
    }

    public int Get(StatName stat)
    {
        switch(stat)
        {
            case StatName.Combat: return this.Combat.Value;
            case StatName.Dungeoneering: return this.Dungeoneering.Value;
            case StatName.Nature: return this.Nature.Value;
            case StatName.Social: return this.Social.Value;
            default: return this.Arcane.Value;
        }
    }

    public float GetMaxValue()
    {
        return 100.0f;
    }
}