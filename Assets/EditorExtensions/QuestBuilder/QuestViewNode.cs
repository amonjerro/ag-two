
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace QuestBuilder {
    public class QuestViewNode : Node
    {

        public QuestNodeData questNode;
        public Port input;
        public List<Port> outputPorts;
        public Action<QuestViewNode> OnNodeSelected;
        public QuestViewNode(QuestNodeData node)
        {
            outputPorts = new List<Port>();
            questNode = node;
            style.left = questNode.positionX;
            style.right = questNode.positionY;

            CreateInputPorts();
            CreateOutputPorts();

        }

        private void CreateInputPorts()
        {
            NodeTypes type = QuestController.MapStringToType(questNode.type);
            switch (type)
            {
                case NodeTypes.Start:
                    input = null;
                    break;
                default:
                    input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
                    break;
            }
            if (input != null)
            {
                input.portName = "";
                inputContainer.Add(input);
            }
        }

        private void CreateOutputPorts() {
            NodeTypes type = QuestController.MapStringToType(questNode.type);
            Port outputPort = null;
            switch (type) {
                case NodeTypes.End:
                    break;
                case NodeTypes.Decision:
                case NodeTypes.Challenge:
                    outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
                    outputPort.portName = "Option 1";
                    outputPorts.Add(outputPort);

                    Port secondOutput = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
                    secondOutput.portName = "Option 2";
                    outputPorts.Add(secondOutput);
                    break;
                default:
                    outputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
                    outputPorts.Add(outputPort);
                    break;
            }
            foreach (Port port in outputPorts) {
                outputContainer.Add(port);
            }
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            questNode.positionX = newPos.x;
            questNode.positionY = newPos.y;
        }

        // Override base event with our logic
        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }
    }
}