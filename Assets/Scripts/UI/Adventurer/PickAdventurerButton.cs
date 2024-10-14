using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAdventurerButton : MonoBehaviour
{
    AAdventurerCommand command;

    public void Execute()
    {
        if (command == null)
        {
            throw new System.NullReferenceException("Command for PickAdventurer Button not set");
        }

        command.Execute();
    }

    public void SetCommand(AAdventurerCommand command)
    {
        this.command = command;
    }
}
