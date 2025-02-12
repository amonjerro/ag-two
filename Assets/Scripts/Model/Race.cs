public enum RaceType {
    Human,
    Kedi,
    Corvo
}

public class RaceFactory{

    public static Stats GetRaceStats(RaceType race){
        switch (race){
            case (RaceType.Human):
                return new Stats(0,0,0,10,0);
            case (RaceType.Kedi):
                return new Stats(0,10,0,0,0);
            case (RaceType.Corvo):
                return new Stats(0,0,10,0,0);
            default:
                return new Stats(0,0,0,0,0);
        }
    }

    public static string GetRaceName(RaceType race){
        switch (race){
            case (RaceType.Human):
                return "Human";
            case (RaceType.Kedi):
                return "Kedi";
            case (RaceType.Corvo):
                return "Corvo";
            default:
                return "Doppelganger";
        }
    }

}