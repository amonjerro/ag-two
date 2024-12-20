using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestNodePanel : QuestUIPanel
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
    CommandButton singleConfirm;

    [SerializeField]
    CommandButton toRosterButton;

    [SerializeField]
    CommandButton Decision1;

    [SerializeField]
    CommandButton Decision2;


    private void Start()
    {
        QuestController qc = ServiceLocator.Instance.GetService<AdventurerManager>().GetQuestController();
        singleConfirm.SetCommand(new AcknowledgeCommand(qc,this));
        Decision1.SetCommand(new DecisionAcceptCommand(qc, this));
        Decision2.SetCommand(new DecisionRejectCommand(qc, this));
        toRosterButton.SetCommand(new NextPageCommand(this));

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

    public override void SetQuestNode(AQuestNode nodeData)
    {
        node = nodeData;
        title.text = node.Title;
        description.text = node.Description;
        DetermineButtonLayout(node.NodeType);
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
            InformationNode info = (InformationNode) node;
            childText.text = info.AcknowledgeString;
        } else
        {
            TextMeshProUGUI childText;
            DecisionNode decNode = (DecisionNode)node;
            if (node.NodeType == NodeTypes.Start) {
                toRosterButton.gameObject.SetActive(true);
                Decision1.gameObject.SetActive(false);
            } else
            {
                toRosterButton.gameObject.SetActive(false);
                Decision1.gameObject.SetActive(true);
                childText = Decision1.GetComponentInChildren<TextMeshProUGUI>();
                childText.text = decNode.Option1String;
            }
            
            Decision2.gameObject.SetActive(true);
            childText = Decision2.GetComponentInChildren<TextMeshProUGUI>();
            childText.text = decNode.Option2String;
            singleConfirm.gameObject.SetActive(false);
        }

        
    }

    public void CyclePage()
    {
        GetComponentInParent<QuestEventUI>().NextPage();
    }

    public AQuestNode GetOpenNode()
    {
        return node;
    }

    public override void Dismiss()
    {
        GetComponentInParent<QuestEventUI>().Dismiss();
    }
}
