using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace QuestBuilder
{
    public class FactionViewNode : GraphTreeNode
    {

        private FactionQuestTreeNode nodeData;
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
    }
}