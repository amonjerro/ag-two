
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueEditor
{
    public class DialogueViewNode : Node
    {

        public DialogueNodeData nodeData;
        public Port input;
        public List<Port> outputPorts;
        public Action<DialogueViewNode> OnNodeSelected;
        public DialogueViewNode Parent { get; set; }

        List<DialogueViewNode> childrenNodes;
        public DialogueViewNode(DialogueNodeData node)
        {
            outputPorts = new List<Port>();
            childrenNodes = new List<DialogueViewNode>();
            nodeData = node;

            title = node.name;
            style.left = nodeData.position.x;
            style.top = nodeData.position.y;

            CreateUtilities();

            CreateInputPorts();
            CreateOutputPorts();
        }

        private void CreateUtilities()
        {
            Button button = new Button();
            button.clicked += CreateNewOutputPort;
            button.tooltip = "Creates a new output port for this node, allowing choices to be made";
            button.text = "Add Output Port";
            mainContainer.Add(button);

        }

        private void CreateInputPorts()
        {

        }

        private void CreateOutputPorts()
        {
            foreach (Port port in outputPorts)
            {
                outputContainer.Add(port);
            }
        }

        private void CreateNewOutputPort()
        {
            Port port = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            port.portName = $"Option {outputPorts.Count}";
            outputPorts.Add(port);
            outputContainer.Add(port);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            nodeData.position.x = newPos.xMin;
            nodeData.position.y = newPos.yMin;
        }

        // Override base event with our logic
        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }


        public void AddChildConnection(DialogueViewNode node, int portNumber)
        {
            childrenNodes[portNumber] = node;
        }

        public int GetPortNumber(Port port)
        {
            return outputPorts.IndexOf(port);
        }

        public int RemoveChild(DialogueViewNode child)
        {
            int indexOf = childrenNodes.IndexOf(child);
            childrenNodes[indexOf] = null;
            return indexOf;
        }
    }
}