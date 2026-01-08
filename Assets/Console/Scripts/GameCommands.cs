using System.Collections.Generic;
using UnityEngine;

public class GameCommands : MonoBehaviour
{
    public CommandsSO commandsSO;

    // ----------------------------------------------------------
    // ALL PARAMETERS AND VARIABLES FOR COMMANDS TO USE
    // ----------------------------------------------------------

    public TMPro.TextMeshProUGUI consoleOutput;

    public LifeimeController lifetimeController;

    void Awake()
    {
        foreach (var command in commandsSO.allCommands)
        {
            RegisterCommand(command);
        }
    }

    void RegisterCommand(NewCommandSO command)
    {
        switch (command.commandName.ToLower())
        {
            case "clear":
                CommandRegistry.Register(command, ClearConsole);
                break;

            case "help":
                CommandRegistry.Register(command, Help);
                break;
            case "des":
                CommandRegistry.RegisterParams<float>(command, t => DestroyObject(t));
                break;
        }
    }

    void ClearConsole()
    {
        consoleOutput.text = string.Empty;
    }

    void Help()
    {
        foreach (var command in commandsSO.allCommands)
        {
            consoleOutput.text += $"<color=white>{command}</color>\n";
        }
    }

    void DestroyObject(float time)
    {
        lifetimeController.StartLifetime(time);
    }
}
