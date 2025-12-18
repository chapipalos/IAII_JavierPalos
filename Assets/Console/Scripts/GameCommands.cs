using UnityEngine;

public class GameCommands : MonoBehaviour
{
    public TMPro.TextMeshProUGUI consoleOutput;

    public CommandsSO commandsSO;

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
            case "cls":
                CommandRegistry.Register(command, ClearConsole);
                break;

            case "help":
                CommandRegistry.Register(command, Help);
                break;
        }
    }

    void ClearConsole(string[] args)
    {
        consoleOutput.text = string.Empty;
    }

    void Help(string[] args)
    {
        foreach (var command in commandsSO.allCommands)
        {
            consoleOutput.text += $"{command}\n";
        }
    }
}
