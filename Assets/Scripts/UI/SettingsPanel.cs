using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : UIPanel
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Show()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public override void Dismiss()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

    }
}
