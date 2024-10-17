using TMPro;
using UnityEngine;

public class QuestButton : MonoBehaviour
{
    AQuestCommand command;

    public void RunCommand()
    {
        if (command == null)
        {
            throw new System.NullReferenceException("Command for Quest Button not set");
        }

        command.Execute();
    }

    public void SetCommand(AQuestCommand command)
    {
        this.command = command;
    }

    public void SetText(string value)
    {
        TextMeshProUGUI buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = value;
    }
}