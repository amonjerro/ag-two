using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAdventurerButton : MonoBehaviour
{
    AbstractCommand command;

    public void Execute()
    {
        if (command == null)
        {
            throw new System.NullReferenceException("Command for PickAdventurer Button not set");
        }

        command.Execute();
    }

    public void SetCommand(AbstractCommand command)
    {
        this.command = command;
    }
}
