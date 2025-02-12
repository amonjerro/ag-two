using UnityEngine;

[CreateAssetMenu(menuName=Constants.ScriptableObjectsPath+"SkyGradients")]
public class SeasonalSkiesSO : ScriptableObject
{
    
    public SeasonGradients summer;
    public SeasonGradients spring;
    public SeasonGradients fall;
    public SeasonGradients winter;

    public SeasonGradients GetBySeason(Season s) {
        switch (s)
        {
            case Season.Summer:
                return summer;
            case Season.Winter:
                return winter;
            case Season.Fall:
                return fall;
            case Season.Spring:
            default:
                return spring;
        }
    }
}

[System.Serializable]
public class SeasonGradients
{
    public Gradient Top;
    public Gradient Bottom;
}
