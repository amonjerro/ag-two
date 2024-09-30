using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestNodePanel : UIPanel
{
    [SerializeField]
    Image image;

    [SerializeField]
    TextMeshProUGUI title;

    [SerializeField]
    TextMeshProUGUI description;

    private bool showingSingleConfirm = false;

    [SerializeField]
    Button singleConfirm;

    [SerializeField]
    Button Decision1;

    [SerializeField]
    Button Decision2;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    private void DetermineButtonLayout(NodeTypes type)
    {
        switch (type)
        {
            case NodeTypes.Decision:
            case NodeTypes.Challenge:
                showingSingleConfirm = false;
                break;
            default:
                showingSingleConfirm = true;
                break;
        }
    }

    public void SetData(AQuestNode nodeData)
    {
        title.text = nodeData.Title;
        description.text = nodeData.Description;
        DetermineButtonLayout(nodeData.NodeType);
    }

    // Start is called before the first frame update
    public override void Show()
    {
        if (showingSingleConfirm)
        {
            Decision1.gameObject.SetActive(false);
            Decision2.gameObject.SetActive(false);
            singleConfirm.gameObject.SetActive(true);
        } else
        {
            Decision1.gameObject.SetActive(true);
            Decision2.gameObject.SetActive(true);
            singleConfirm.gameObject.SetActive(false);
        }

        animator.SetBool("bShow", true);
    }



    public override void Dismiss()
    {
        animator.SetBool("bShow", false);
    }
}
