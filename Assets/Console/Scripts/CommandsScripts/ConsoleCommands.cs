using TMPro;
using UnityEngine;

public static class ConsoleCommands
{

    public delegate void ClearConsole(TextMeshProUGUI consoleOutput);

    public delegate void HelpCommand(TextMeshProUGUI consoleOutput, CommandsSOController allCommands);
}
