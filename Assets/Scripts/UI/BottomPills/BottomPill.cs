using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomPill : MonoBehaviour
{
    [SerializeField]
    UIPanel ownedPanel;

    public void OpenPanel()
    {
        ownedPanel.Show();
    }
}
