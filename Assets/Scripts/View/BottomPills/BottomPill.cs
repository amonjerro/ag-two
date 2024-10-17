using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomPill : UIPanel
{
    [SerializeField]
    UIPanel ownedPanel;

    public void OpenPanel()
    {
        ownedPanel.Show();
    }

    public override void Show()
    {
        
    }

    public override void Dismiss()
    {
        throw new System.NotImplementedException();
    }
}
