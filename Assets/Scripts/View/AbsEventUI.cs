using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbsEventUI : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Dismiss()
    {
        gameObject.SetActive(false);
    }
}
