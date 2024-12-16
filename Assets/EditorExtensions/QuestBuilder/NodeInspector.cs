using System;
using QuestBuilder;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuestBuilder
{
    public class NodeInspector : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<NodeInspector, VisualElement.UxmlTraits> { }

        Editor editor;
        TextField titleField;
        TextField keyField;
        TextField descriptionField;
        Foldout buttonStrings;
        IntegerField durationField;
        QuestViewNode activeNode;

        public NodeInspector()
        {
        }


        public void Init()
        {

            keyField = new TextField("Node Key");
            keyField.RegisterValueChangedCallback(HandleChange);
            keyField.name = "key";
            keyField.isReadOnly = true;
            Add(keyField);


            titleField = new TextField("Title");
            titleField.RegisterValueChangedCallback(HandleChange);
            titleField.name = "title";
            Add(titleField);


            descriptionField = new TextField("Description");
            descriptionField.multiline = true;
            descriptionField.RegisterValueChangedCallback(HandleChange);
            descriptionField.name = "description";
            Add(descriptionField);

            durationField = new IntegerField("Duration");
            durationField.RegisterValueChangedCallback(HandleChange);
            durationField.name = "duration";
            Add(durationField);

            buttonStrings = new Foldout { text = "Button Strings" };
            Add(buttonStrings);


        }

        public void UpdateSelection(QuestViewNode viewNode)
        {
            activeNode = viewNode;
            keyField.value = viewNode.questNode.key;
            titleField.value = viewNode.questNode.title;
            descriptionField.value = viewNode.questNode.description;
            durationField.value = viewNode.questNode.duration;

            // clear the button string view
            buttonStrings.Clear();

            for (int i = 0; i < QuestController.TypeToButtonStrings(QuestController.MapStringToType(viewNode.questNode.type)); i++)
            {
                TextField textField = new TextField($"Button String {i + 1}");
                textField.RegisterValueChangedCallback(HandleChange);
                textField.name = $"child-{i}";
                textField.value = viewNode.questNode.buttonStrings[i];
                buttonStrings.Add(textField);
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

        private void HandleChange(ChangeEvent<int> evt)
        {
            activeNode.questNode.duration = evt.newValue;
        }

        private void ApplyChange(string inputName, string newValue)
        {
            switch (inputName)
            {

                case "title":
                    activeNode.title = newValue;
                    activeNode.questNode.title = newValue;
                    break;
                case "description":
                    activeNode.questNode.description = newValue;
                    break;
                case "key":
                    break;
                default:
                    int childIndex = Int32.Parse(inputName.Substring(inputName.IndexOf('-') + 1));
                    activeNode.questNode.buttonStrings[childIndex] = newValue;
                    break;
            }
        }
    }
}
