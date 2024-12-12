using UnityEngine;

public class LeafSceneTransitionManager : MainSceneTransitionManager
{
    private void Start()
    {
        // Unfade the panel
        StartCoroutine(fadePanel.FadeOut());
    }

    public void FadeToScene()
    {
        StartCoroutine(LoadScene(1));
    }
}