using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class FadePanel : MonoBehaviour
{
    private Image panelImage;

    private void Awake()
    {
        panelImage = GetComponent<Image>();
    }

    public void SetOpacity(float opacity)
    {
        panelImage.color = new Color(0,0,0,opacity);
    }

    public IEnumerator FadeOut()
    {
        float t = 1;
        while(t > 0f)
        { 
            t -= Time.deltaTime / 2;
            t = Mathf.Clamp01(t);
            SetOpacity(t);
            yield return null;
        }
        transform.SetAsFirstSibling();
    }
}