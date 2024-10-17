using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FactionPanel : UIPanel
{
    public Image factionImage;
    public TextMeshProUGUI factionTitle;

    public override void Show()
    {
        
    }

    public override void Dismiss()
    {
        throw new System.NotImplementedException();
    }

    public void SetInformation(FactionDetails data)
    {
        factionImage.sprite = data.factionSprite;
        factionTitle.text = data.factionName;
    }
}