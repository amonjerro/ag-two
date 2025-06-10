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
        }

        protected override void CreateInputPorts()
        {
            throw new System.NotImplementedException();
        }

        protected override void CreateOutputPorts()
        {
            throw new System.NotImplementedException();
        }

        protected override void SetLocationData(float xPosition, float yPosition)
        {
            throw new System.NotImplementedException();
        }

        public override string GetKey()
        {
            return nodeData.questKey;
        }
    }
}