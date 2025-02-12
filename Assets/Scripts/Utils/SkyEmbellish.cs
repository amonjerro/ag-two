using UnityEngine;

public class SkyEmbellish : MonoBehaviour
{
    [SerializeField]
    SeasonalSkiesSO skyGradientData;

    [SerializeField]
    Shader skyShader;

    [SerializeField]
    SpriteRenderer skyRenderer;


    TimeManager timeManagerReference;

    public void Start()
    {
        TimeManager.Tick += UpdateSky;
        timeManagerReference = ServiceLocator.Instance.GetService<TimeManager>();
        UpdateSky();
    }

    private void UpdateSky()
    {
        SeasonGradients currentGradients = skyGradientData.GetBySeason(timeManagerReference.GetCurrentSeason());
        float dayProgress = timeManagerReference.GetCurrentDayProgress();
        skyRenderer.material.SetColor("_TopColor", currentGradients.Top.Evaluate(dayProgress));
        skyRenderer.material.SetColor("_BottomColor", currentGradients.Bottom.Evaluate(dayProgress));
    }

}