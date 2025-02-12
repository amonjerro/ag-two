public enum SpeciesType {
    Human,
    Kedi,
    Corvo
}

public class SpeciesFactory{

    // Creates a new Stats vector corresponding to the bonuses of this species
    public static Stats GetSpeciesStats(SpeciesType species){
        switch (species){
            case (SpeciesType.Human):
                return new Stats(0,0,0,10,0);
            case (SpeciesType.Kedi):
                return new Stats(0,10,0,0,0);
            case (SpeciesType.Corvo):
                return new Stats(0,0,10,0,0);
            default:
                return new Stats(0,0,0,0,0);
        }
    }

    public static string GetSpeciesName(SpeciesType species){
        switch (species){
            case (SpeciesType.Human):
                return "Human";
            case (SpeciesType.Kedi):
                return "Kedi";
            case (SpeciesType.Corvo):
                return "Corvo";
            default:
                return "Doppelganger";
        }
    }

}