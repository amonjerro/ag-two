using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestNodePanel : UIPanel
{
    [SerializeField]
    UIPanel sidePanel;

    [SerializeField]
    Image image;

    [SerializeField]
    TextMeshProUGUI title;

    [SerializeField]
    TextMeshProUGUI description;

    AQuestNode node;

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

    public void Acknowledge()
    {
        // Test to see if we're done
        if (node.Next == null)
        {

        } else
        {
            // Set next consequence
        }


        // Set next consequence
        Dismiss();
    }

    public void ChooseDecisionOne()
    {
        if (node.NodeType == NodeTypes.Start) { 
            // Change UI to be Adventurer Assign
        } else
        {
            // Set next consequence

            // Make this message go away
            Dismiss();
        }
    }

    public void ChooseDecisionTwo()
    {
        try
        {
            DecisionNode decNode = (DecisionNode)node;
            if (decNode.Option2 == null)
            {
                Dismiss();
            } else
            {

            }
        } catch(System.InvalidCastException e) {
            Debug.Log("A non-decision node has spawned decision buttons");
            Debug.LogError(e.Message);
        }
    }

    private void DetermineButtonLayout(NodeTypes type)
    {
        switch (type)
        {
            case NodeTypes.Start:
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
        node = nodeData;
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
            TextMeshProUGUI childText = singleConfirm.GetComponentInChildren<TextMeshProUGUI>();
            InformationNode info = (InformationNode)node;
            childText.text = info.AcknowledgeString;
        } else
        {
            DecisionNode decNode = (DecisionNode)node;
            Decision1.gameObject.SetActive(true);
            TextMeshProUGUI childText = Decision1.GetComponentInChildren<TextMeshProUGUI>();
            childText.text = decNode.Option1String;
            Decision2.gameObject.SetActive(true);
            childText = Decision2.GetComponentInChildren<TextMeshProUGUI>();
            childText.text = decNode.Option2String;
            singleConfirm.gameObject.SetActive(false);
        }

        animator.SetBool("bShow", true);
    }

    public override void Dismiss()
    {
        sidePanel.Dismiss();
        animator.SetBool("bShow", false);
    }
}
