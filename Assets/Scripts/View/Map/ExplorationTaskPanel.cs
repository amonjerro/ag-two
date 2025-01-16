using UnityEngine;

namespace ExplorationMap {
    public class ExplorationTaskPanel : UIPanel
    {
        Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public override void Show()
        {
            animator.SetBool("bShowing", true);
        }

        public override void Dismiss()
        {
            animator.SetBool("bShowing", false);
        }
    }
}
