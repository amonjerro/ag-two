using Rooms;
using SaveGame;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneTransitionManager : MonoBehaviour
{
    public FadePanel fadePanel;
    TimeManager timeManager;
    private void Start()
    {
        // Load the roster

        // Ensure all rooms load properly
        RoomManager roomManager = ServiceLocator.Instance.GetService<RoomManager>();
        roomManager.LoadTasks(GameInstance.tasksToPopulate);

        // Ensure all quest data is properly loaded to the UI and controllers

        // Unfade the panel
        StartCoroutine(fadePanel.FadeOut());

        // Start the clock
        timeManager = ServiceLocator.Instance.GetService<TimeManager>();
        timeManager.LoadTime(GameInstance.totalTickCount);

        
    }

    protected IEnumerator LoadScene(int scene)
    {

        fadePanel.transform.SetAsLastSibling();
        // Begin to load the scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);


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

    protected virtual void SaveDataForTransition()
    {
        timeManager.IsPaused = true;
        GameInstance.totalTickCount = timeManager.GetTimeToSave();
    }

    public void FadeToScene(int sceneIndex)
    {
        SaveDataForTransition();
        StartCoroutine(LoadScene(sceneIndex));
    }
}