using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuestBuilder
{
    public class FactionViewNode : GraphTreeNode
    {

        private FactionQuestTreeNode nodeData;
        private Label lInputMode;
        public FactionViewNode Parent { get; set; }
        public FactionQuestTreeNode NodeData { get { return nodeData; } private set { nodeData = value; } }
        

        public FactionViewNode(FactionQuestTreeNode incomingData)
        {
            NodeData = incomingData;
            Initialize();
        }

        protected override void Initialize()
        {
            CreateUtilities();

            for (int i = 0; i < nodeData.children.Count; i++) {
                CreateOutputPort();
            }

            CheckQuestStatus();
            InstallManipulator(this);
        }

        private void CreateUtilities()
        {
            Button inputButton = new Button();
            inputButton.tooltip = "Creates a new input port for this node, indicating what quests unlock this one";
            inputButton.text = "Add Input Port";
            inputButton.clicked += CreateInputPort;
            mainContainer.Add(inputButton);

            Button outputButton = new Button();
            outputButton.tooltip = "Creates a new output port for this node, which indicates which quests it opens";
            outputButton.text = "Add Output Port";
            outputButton.clicked += CreateOutputPort;
            mainContainer.Add(outputButton);

            lInputMode = new Label("AND");
            lInputMode.style.paddingLeft = 10;
            inputContainer.Add(lInputMode);

            Button inputModeToggleButton = new Button();
            inputModeToggleButton.tooltip = "Toggles the input requirement type";
            inputModeToggleButton.text = "Toggle Input Mode";
            inputModeToggleButton.clicked += ToggleInputMode;
            mainContainer.Add(inputModeToggleButton);
        }


        private void CreateOutputPort()
        {
            Port p = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            p.portName = "";
            outputContainer.Add(p);
        }

        private void CreateInputPort()
        {
            

            Port p = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            p.portName = "";
            inputContainer.Add(p);
        }

        protected override void SetLocationData(float xPosition, float yPosition)
        {
            nodeData.positionX = xPosition;
            nodeData.positionY = yPosition;
        }

        public override string GetKey()
        {
            return nodeData.questKey;
        }

        private void ToggleInputMode()
        {
            lInputMode.text = nodeData.ToggleInputMode().ToString();
        }

        protected override void MenuBuildingDelegate(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Remove Node", (a) => {
                Debug.Log(GetKey());
            });
        }

        private void CheckQuestStatus()
        {
            // Check if the underlying json file for the nodes exists at all
            string lookupPath = $"{Application.dataPath}/{QuestSystemConstants.QuestNodeLocations}/{nodeData.questKey}.json";
            if (!File.Exists(lookupPath)) {
                SetStyleClass("unpopulated");
                return;
            }

            // If it exists, check if it is valid
            // To do: Add some validation rules here

            SetStyleClass("populated");
        }
    }
}