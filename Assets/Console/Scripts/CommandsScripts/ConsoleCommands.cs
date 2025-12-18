using TMPro;
using UnityEngine;

public class ConsoleCommands : MonoBehaviour
{
    public TextMeshProUGUI consoleOutput;

    public void ClearConsole()
    {
        consoleOutput.text = string.Empty;
    }
}
