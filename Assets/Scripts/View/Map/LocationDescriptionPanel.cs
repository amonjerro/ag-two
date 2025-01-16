using TMPro;
using UnityEngine;

namespace ExplorationMap
{
    public class LocationDescriptionPanel : UIPanel
    {
        [SerializeField]
        TextMeshProUGUI title;

        [SerializeField]
        TextMeshProUGUI description;

        [SerializeField]
        VerticalButtonContainer buttonContainer;

        Animator animator;
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public override void Dismiss()
        {
            animator.SetBool("bShow", false);
        }

        public override void Show()
        {
            animator.SetBool("bShow", true);
        }
    }
}