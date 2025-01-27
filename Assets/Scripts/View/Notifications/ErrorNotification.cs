using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ErrorNotification : UIPanel
{
    Image imageComponent;
    [SerializeField]
    Color active;


    [SerializeField]
    TextMeshProUGUI errorMessage;

    private void Start()
    {
        imageComponent = GetComponent<Image>();
    }

    public void SetMessage(string text)
    {
        errorMessage.text = text;
    }

    public override void Show()
    {
        imageComponent.color = active;
        StartCoroutine(WaitForDismiss());
    }

    public override void Dismiss()
    {
        StartCoroutine(ErrorFade());
    }

    IEnumerator WaitForDismiss()
    {
        yield return new WaitForSeconds(3);
        Dismiss();
    }

    IEnumerator ErrorFade()
    {
        float t = 1;
        while(t > 0f)
        {
            t -= Time.deltaTime;
            t = Mathf.Clamp01(t);
            imageComponent.color = new Color(active.r, active.g, active.b, t);
            yield return null;
        }
        errorMessage.text = "";
    }
}