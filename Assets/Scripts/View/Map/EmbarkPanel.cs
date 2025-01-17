
using UnityEngine;

namespace ExplorationMap
{
    public class EmbarkPanel : UIPanel
    {
        Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }


        public override void Dismiss()
        {
            animator.SetBool("bShowing", false);
        }

        public override void Show()
        {
            animator.SetBool("bShowing", true);
        }
    }
}