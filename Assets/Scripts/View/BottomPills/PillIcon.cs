using UnityEngine;

public class PillIcon : MonoBehaviour
{
    public void DismissParent()
    {
        GetComponentInParent<UIPanel>().Dismiss();
    }
}