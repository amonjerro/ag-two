using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : UIPanel
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Show()
    {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
        }
    }

    public override void Dismiss()
    {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
