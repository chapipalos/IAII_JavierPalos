using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CommandsController : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI consoleOutput;

    public InputActionReference toggleConsoleAction;

    private void Awake()
    {
        toggleConsoleAction.action.Enable();
        toggleConsoleAction.action.performed += ctx => ToggleActionConsole();
    }

    private void ToggleActionConsole()
    {
        if (!string.IsNullOrWhiteSpace(inputField.text))
        {
            if (CommandRegistry.IsRegistered(inputField.text.Split(' ', System.StringSplitOptions.RemoveEmptyEntries)[0]))
            {
                consoleOutput.text += $"\n> {inputField.text}\n";
                CommandRegistry.Execute(inputField.text);
                inputField.text = string.Empty;
                inputField.ActivateInputField();
            }
            else
            {
                consoleOutput.text += $"Command not found: {inputField.text}\n";
                inputField.text = string.Empty;
                inputField.ActivateInputField();
            }
        }
    }
}
