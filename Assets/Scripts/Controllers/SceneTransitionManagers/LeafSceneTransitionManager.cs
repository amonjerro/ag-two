public class LeafSceneTransitionManager : MainSceneTransitionManager
{
    private void Start()
    {
        // Unfade the panel
        if (fadePanel.gameObject.activeInHierarchy)
        {
            StartCoroutine(fadePanel.FadeOut());
        }
    }

    public void FadeToScene()
    {
        StartCoroutine(LoadScene(1));
    }
}