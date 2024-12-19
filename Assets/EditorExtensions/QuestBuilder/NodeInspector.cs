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
        Toggle providesRewards;
        Foldout buttonStrings;
        Foldout challengeValues;
        IntegerField arcaneChallenge;
        IntegerField socialChallenge;
        IntegerField dungeoneeringChallenge;
        IntegerField natureChallenge;
        IntegerField combatChallenge;


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

            providesRewards = new Toggle("Provides Rewards");
            providesRewards.RegisterValueChangedCallback(HandleChange);
            Add(providesRewards);

            challengeValues = new Foldout { text = "Challenge Values" };
            Add(challengeValues);

            arcaneChallenge = new IntegerField("Arcane");
            arcaneChallenge.RegisterValueChangedCallback(HandleChange);
            arcaneChallenge.name = "arc";
            challengeValues.Add(arcaneChallenge);

            socialChallenge = new IntegerField("Social");
            socialChallenge.RegisterValueChangedCallback(HandleChange);
            socialChallenge.name = "soc";
            challengeValues.Add(socialChallenge);

            dungeoneeringChallenge = new IntegerField("Dungeoneering");
            dungeoneeringChallenge.RegisterValueChangedCallback(HandleChange);
            dungeoneeringChallenge.name = "dun";
            challengeValues.Add(dungeoneeringChallenge);

            natureChallenge = new IntegerField("Nature");
            natureChallenge.RegisterValueChangedCallback(HandleChange);
            natureChallenge.name = "nat";
            challengeValues.Add(natureChallenge);

            combatChallenge = new IntegerField("Combat");
            combatChallenge.RegisterValueChangedCallback(HandleChange);
            combatChallenge.name = "com";
            challengeValues.Add(combatChallenge);

            

            challengeValues.visible = false;
            providesRewards.visible = false;
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

            //clear the challenge values
            challengeValues.visible = viewNode.nodeType == NodeTypes.Challenge;
            providesRewards.visible = viewNode.nodeType == NodeTypes.End;

            if (challengeValues.visible)
            {
                arcaneChallenge.value = viewNode.questNode.challengeValues.arcane;
                socialChallenge.value = viewNode.questNode.challengeValues.social;
                dungeoneeringChallenge.value = viewNode.questNode.challengeValues.dungeoneering;
                natureChallenge.value = viewNode.questNode.challengeValues.nature;
                combatChallenge.value = viewNode.questNode.challengeValues.combat;
            }

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
            switch (inputName) {
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
