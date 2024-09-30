public class LevellingController {

    private static readonly int[] levelBreaks = {0,0,1000,3000,6000,10000,15000,21000,28000,36000,450000};
    public static bool ShouldLevelUp(int xp, int current_level){
        // Already at max level
        if (current_level == 10){
            return false;
        }

        return xp > levelBreaks[current_level];
    }

}