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
    }

    private void UpdateSky()
    {
        skyRenderer.material.SetColor("_TopColor", Color.blue);
        skyRenderer.material.SetColor("_BottomColor", Color.blue);
    }

}