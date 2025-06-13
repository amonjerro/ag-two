
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace QuestBuilder {
    public class QuestViewNode : GraphTreeNode
    {

        public QuestNodeData questNode;
        
        public QuestViewNode Parent { get; set; }
        public NodeTypes nodeType;
        public QuestViewNode(QuestNodeData node)
        {
            questNode = node;
            Initialize();
        }

        protected override void Initialize()
        {
            outputPorts = new List<Port>();
            childrenNodes = new List<GraphTreeNode>();

            title = questNode.title;
            style.left = questNode.positionX;
            style.top = questNode.positionY;
            nodeType = QuestController.MapStringToType(questNode.type);

            CreateInputPorts();
            CreateOutputPorts();
        }

        private void CreateInputPorts()
        {
            inputPorts = new List<Port>();
            switch (nodeType)
            {
                case NodeTypes.Start:
                    break;
                default:
                    inputPorts.Add(InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool)));
                    break;
            }
            if (inputPorts.Count > 0)
            {
                inputPorts[0].portName = "";
                inputContainer.Add(inputPorts[0]);
            }
        }

        private void CreateOutputPorts() {
            Port outputPort = null;
            switch (nodeType) {
                case NodeTypes.End:
                    break;
                case NodeTypes.Decision:
                case NodeTypes.Challenge:
                case NodeTypes.Start:
                    outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                    outputPort.portName = "Option 1";
                    outputPorts.Add(outputPort);

                    Port secondOutput = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                    secondOutput.portName = "Option 2";
                    outputPorts.Add(secondOutput);

                    childrenNodes.Add(null);
                    childrenNodes.Add(null);
                    break;
                default:
                    outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                    outputPort.portName = "";
                    outputPorts.Add(outputPort);
                    childrenNodes.Add(null);
                    break;
            }
            foreach (Port port in outputPorts) {
                outputContainer.Add(port);
            }
        }

        protected override void SetLocationData(float xPosition, float yPosition)
        {
            questNode.positionX = xPosition;
            questNode.positionY = yPosition;
        }

        public override string GetKey()
        {
            return questNode.key;
        }

        protected override void MenuBuildingDelegate(ContextualMenuPopulateEvent evt)
        {
            throw new NotImplementedException();
        }
    }
}