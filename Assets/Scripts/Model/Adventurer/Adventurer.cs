
/// <summary>
/// Struct to determine how to construct this adventurer from an assortment of sprites
/// </summary>
public struct AdventurerSpriteIndeces
{
    public int Head;
    public int Body;
    public int Torso;
    public int Legs;

    public AdventurerSpriteIndeces(int h, int b, int t, int l)
    {
        Head = h;
        Body = b;
        Torso = t;
        Legs = l;
    }
}

public class Adventurer
{
    public string Name {get; set;}
    public int Level { get; private set; }
    public Stats Char_Stats { get; private set; }
    public string species;

    private int _xp;
    private bool _onMission;
    private AdventurerSpriteIndeces spriteConfig;
    
    public Adventurer(){

        Char_Stats = new Stats(0,0,0,0,0);
        Level = 1;
        _onMission = false;
        _xp = 0;
    }

    public Adventurer(string name, SpeciesType species){
        Char_Stats = new Stats(0,0,0,0,0);
        Char_Stats.Add(SpeciesFactory.GetSpeciesStats(species));
        this.species = SpeciesFactory.GetSpeciesName(species);
        Level = 1;
        _onMission = false;
        _xp = 0;
        Name = name;
    }

    /// <summary>
    /// Add an amount of experience to this adventurers total
    /// </summary>
    /// <param name="xp">How much XP to add</param>
    public void AssignXp(int xp){
        _xp += xp;
        if (LevellingController.ShouldLevelUp(_xp, Level)){
            //Perform level up operation
            Level++;
            AdjustSkills();
        }
    }

    /// <summary>
    /// Adjust the level of this adventurer to some value
    /// </summary>
    /// <param name="level">Value to set the level to</param>
    public void LevelAdjust(int level){
        for (int i = 0; i < (level - this.Level); i++ ){
            AdjustSkills();
        }
        this.Level = level;
    }

    public void AdjustSkills(){
        //TO DO
    }

    /// <summary>
    /// Returns if on mission
    /// </summary>
    /// <returns></returns>
    public bool IsOnMission(){
        return _onMission;
    }

    /// <summary>
    /// Sets _onMission to be true
    /// </summary>
    public void SendOnMission(){
        _onMission = true;
    }


    /// <summary>
    /// Sets _onMission to be false
    /// </summary>
    public void ReturnFromMission()
    {
        _onMission = false;
    }


    // Debug function
    public override string ToString(){
        return "Name: "+this.Name+" Species: "+this.species;
    }

}
