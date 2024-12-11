using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public FadePanel fadePanel;
    public UIPanel SettingsPanel;
    public UIPanel MainMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadScene(int scene)
    {
        // Begin to load the scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);


        // Don't allow the scene to activate until we're read
        operation.allowSceneActivation = false;

        float t = 0;
        while (operation.progress < 0.9f || t < 1.0f)
        {
            t += Time.deltaTime;
            t = Mathf.Clamp01(t);
            fadePanel.SetOpacity(t);
            yield return null;
        }
        operation.allowSceneActivation = true;
    }

    public void StartGame()
    {
        fadePanel.transform.SetAsLastSibling();
        StartCoroutine(LoadScene(1));
    }

    public void OpenSettings()
    {
        SettingsPanel.Show();
        MainMenu.Dismiss();
    }

    public void CloseSettings()
    {
        SettingsPanel.Dismiss();
        MainMenu.Show();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }
}
