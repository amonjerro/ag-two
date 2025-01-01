using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace DialogueEditor
{
    public class DialogueInspector : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<DialogueInspector, VisualElement.UxmlTraits> { }

        Editor editor;
        TextField mainTextField;
        TextField keyField;
        TextField descriptionField;
        Foldout linkDetails;
        DialogueViewNode activeNode;

        public DialogueInspector()
        {
        }


        public void Init()
        {

            keyField = new TextField("Node Key");
            keyField.RegisterValueChangedCallback(HandleChange);
            keyField.name = "key";
            keyField.isReadOnly = true;
            Add(keyField);


            mainTextField = new TextField("Title");
            mainTextField.RegisterValueChangedCallback(HandleChange);
            mainTextField.name = "title";
            Add(mainTextField);


            descriptionField = new TextField("Description");
            descriptionField.multiline = true;
            descriptionField.RegisterValueChangedCallback(HandleChange);
            descriptionField.name = "description";
            Add(descriptionField);

            linkDetails = new Foldout();
            linkDetails.text = "Link Details";
            Add(linkDetails);

        }

        public void UpdateSelection(DialogueViewNode viewNode)
        {
            activeNode = viewNode;
            keyField.value = viewNode.nodeData.pid;
            mainTextField.value = viewNode.nodeData.name;
            descriptionField.value = viewNode.nodeData.text;

            // clear the button string view
            linkDetails.Clear();

            for (int i = 0; i < viewNode.nodeData.links.Count; i++)
            {
                TextField textField = new TextField($"Button String {i + 1}");
                textField.RegisterValueChangedCallback(HandleChange);
                textField.name = $"child-{i}";
                textField.value = viewNode.nodeData.links[i].name;
                linkDetails.Add(textField);
            }

        }

        private void HandleChange(ChangeEvent<string> evt)
        {
            if (activeNode == null)
            {
                return;
            }
            VisualElement ve = (VisualElement)evt.currentTarget;
            ApplyChange(ve.name, evt.newValue);
        }

        private void ApplyChange(string inputName, string newValue)
        {
            switch (inputName)
            {
                case "description":
                    activeNode.nodeData.text = newValue;
                    break;
                case "key":
                    // Key is read only, no changes can be done or should be recorded
                    break;
                case "title":
                    activeNode.title = newValue;
                    break;
                default:
                    int childIndex = Int32.Parse(inputName.Substring(inputName.IndexOf('-') + 1));
                    activeNode.nodeData.UpdateLinkText(childIndex, newValue);
                    break;
            }
        }
    }
}
