using SaveGame;
using UnityEngine;

public class MainSceneTransitionManager : MonoBehaviour
{
    public FadePanel fadePanel;
    private void Start()
    {
        // Load the roster

        // Ensure all rooms load properly

        // Ensure all quest data is properly loaded to the UI and controllers

        // Unfade the panel
        StartCoroutine(fadePanel.FadeOut());

        // Start the clock
        ServiceLocator.Instance.GetService<TimeManager>().LoadTime(GameInstance.tickCount);
    }
}