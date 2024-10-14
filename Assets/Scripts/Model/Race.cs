public enum RaceType {
    Human,
    Elf,
    Dwarf
}

public class RaceFactory{

    public static Stats GetRaceStats(RaceType race){
        switch (race){
            case (RaceType.Human):
                return new Stats(0,0,0,10,0);
            case (RaceType.Elf):
                return new Stats(0,10,0,0,0);
            case (RaceType.Dwarf):
                return new Stats(0,0,10,0,0);
            default:
                return new Stats(0,0,0,0,0);
        }
    }

    public static string GetRaceName(RaceType race){
        switch (race){
            case (RaceType.Human):
                return "Human";
            case (RaceType.Elf):
                return "Elf";
            case (RaceType.Dwarf):
                return "Dwarf";
            default:
                return "Doppelganger";
        }
    }

}