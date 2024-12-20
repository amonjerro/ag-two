using System.Collections.Generic;
using UnityEngine;

public class InterfaceCollection : MonoBehaviour
{
    public List<UIPanel> panels;

    public void SetActive(bool active)
    {
        if (active)
        {
            Activate();
        }
        else {
            Deactivate();
        }
    }

    private void Deactivate()
    {
        foreach (UIPanel p in panels) {
            p.Dismiss();
            p.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    private void Activate()
    {
        gameObject.SetActive(true);
        foreach (UIPanel p in panels)
        {
            p.gameObject.SetActive(true);
            p.Show();
        }
    }
}