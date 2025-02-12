using UnityEngine;


/// <summary>
/// Scriptable object to hold all sky season gradients
/// </summary>
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

/// <summary>
/// Class to hold gradients for the color the sky should be at the top and at the bottom
/// </summary>
[System.Serializable]
public class SeasonGradients
{
    public Gradient Top;
    public Gradient Bottom;
}
