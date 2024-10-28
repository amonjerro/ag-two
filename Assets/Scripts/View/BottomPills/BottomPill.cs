using GameCursor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomPill : UIPanel
{
    [SerializeField]
    UIPanel ownedPanel;

    public CursorStates state;

    public void OpenPanel()
    {
        ServiceLocator.Instance.GetService<CursorManager>().SetCursorState(state);
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
