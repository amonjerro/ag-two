
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuestBuilder {
    public class QuestViewNode : Node
    {

        public QuestNodeData questNode;
        public Port input;
        public List<Port> outputPorts;
        public Action<QuestViewNode> OnNodeSelected;
        public QuestViewNode Parent { get; set; }

        List<QuestViewNode> childrenNodes;
        public QuestViewNode(QuestNodeData node)
        {
            outputPorts = new List<Port>();
            childrenNodes = new List<QuestViewNode>(); 
            questNode = node;
            style.left = questNode.positionX;
            style.top = questNode.positionY;

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
                    input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
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
                    outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                    outputPort.portName = "Option 1";
                    outputPorts.Add(outputPort);

                    Port secondOutput = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                    secondOutput.portName = "Option 2";
                    outputPorts.Add(secondOutput);
                    break;
                default:
                    outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                    outputPort.portName = "";
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

            questNode.positionX = newPos.xMin;
            questNode.positionY = newPos.yMin;
        }

        // Override base event with our logic
        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }

        public void AddChildConnection(QuestViewNode node)
        {
            Debug.Log("Child connection added");
            childrenNodes.Add(node);
        }

        public void UpdateChildrenKey(string guid, string newValue)
        {
            QuestViewNode relevantChild = null;
            foreach (QuestViewNode child in childrenNodes)
            {
                if (child.questNode.key == guid)
                {
                    relevantChild = child;
                    break;
                }
            }
            if (relevantChild != null)
            {
                Debug.Log("Found Child");
                relevantChild.questNode.key = newValue;
            }
        }
    }
}