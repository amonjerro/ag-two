using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace QuestBuilder
{
    public abstract class GraphTreeNode : Node
    {
        public Action<GraphTreeNode> OnNodeSelected;
        public Action OnNodeUnselected;
        public List<Port> outputPorts;
        public List<Port> inputPorts;
        protected List<GraphTreeNode> childrenNodes;

        protected abstract void CreateInputPorts();
        protected abstract void CreateOutputPorts();
        protected abstract void SetLocationData(float xPosition, float yPosition);

        public abstract string GetKey();
        public int RemoveChild(GraphTreeNode child) {
            int indexOf = childrenNodes.IndexOf(child);
            childrenNodes[indexOf] = null;
            return indexOf;
        }

        public void AddChildConnection(GraphTreeNode node, int portNumber)
        {
            childrenNodes[portNumber] = node;
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            SetLocationData(newPos.xMin, newPos.yMin);
        }

        // Override base event with our logic
        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }

        public override void OnUnselected()
        {
            base.OnUnselected();
            OnNodeUnselected?.Invoke();
        }
        public int GetPortNumber(Port port, bool output = true)
        {
            if (output) return outputPorts.IndexOf(port);
            return inputPorts.IndexOf(port);
        }

        public void SetStyleClass(string styleClass)
        {
            mainContainer.AddToClassList(styleClass);
        }
        
    }
}
