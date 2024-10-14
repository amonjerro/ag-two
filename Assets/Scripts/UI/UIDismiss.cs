using UnityEngine;

public class UIDismissTool : UIPanel
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Show()
    {
        animator.SetBool("bShow", true);
    }

    public override void Dismiss()
    {
        animator.SetBool("bShow", false);
    }
}