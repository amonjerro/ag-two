using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace QuestBuilder
{
    public class FactionQuestInspector : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<FactionQuestInspector, VisualElement.UxmlTraits> { }

        Editor editor;
        TextField titleField;
        TextField keyField;
        TextField descriptionField;
        Toggle providesRewards;
        Foldout buttonStrings;
        Foldout challengeValues;


        IntegerField durationField;
        QuestViewNode activeNode;

        public FactionQuestInspector()
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

            providesRewards = new Toggle("Provides Rewards");
            providesRewards.RegisterValueChangedCallback(HandleChange);
            Add(providesRewards);

            challengeValues = new Foldout { text = "Challenge Values" };
            Add(challengeValues);



            challengeValues.visible = false;
            providesRewards.visible = false;
        }

        public void UpdateSelection(FactionViewNode viewNode)
        {
            
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
            if (activeNode == null)
            {
                return;
            }

            VisualElement ve = (VisualElement)evt.currentTarget;
            ApplyChange(ve.name, evt.newValue);
        }

        private void HandleChange(ChangeEvent<bool> evt)
        {
            activeNode.questNode.providesReward = evt.newValue;
        }

        private void ApplyChange(string inputName, int newValue)
        {
            switch (inputName)
            {
                case "com":
                    activeNode.questNode.challengeValues.combat = newValue;
                    break;
                case "dun":
                    activeNode.questNode.challengeValues.dungeoneering = newValue;
                    break;
                case "nat":
                    activeNode.questNode.challengeValues.nature = newValue;
                    break;
                case "soc":
                    activeNode.questNode.challengeValues.social = newValue;
                    break;
                case "arc":
                    activeNode.questNode.challengeValues.arcane = newValue;
                    break;
                default:
                    activeNode.questNode.duration = newValue;
                    break;
            }
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
