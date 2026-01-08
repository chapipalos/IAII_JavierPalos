using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CommandsController : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_InputField autocompleteField;
    public TMP_Text consoleOutput;

    private List<string> previousInput = new List<string>();
    private int previousInputIndex = -1;

    // Input Actions
    public InputActionReference toggleConsoleAction;
    public InputActionReference historialAction;
    public InputActionReference autocompleteAction;

    private void Awake()
    {
        toggleConsoleAction.action.Enable();
        toggleConsoleAction.action.performed += ctx => ToggleActionConsole();

        historialAction.action.Enable();
        historialAction.action.performed += ctx => NavigateHistory(ctx);

        autocompleteAction.action.Enable();
        autocompleteAction.action.performed += ctx => ToggleAutoComplete();
    }

    private void ToggleActionConsole()
    {
        if (!string.IsNullOrWhiteSpace(inputField.text))
        {
            if (CommandRegistry.IsRegistered(inputField.text.Split(' ', System.StringSplitOptions.RemoveEmptyEntries)[0]))
            {
                consoleOutput.text += $"\n -<color=white><noparse>{inputField.text}</noparse></color>\n";

                CommandRegistry.Execute(inputField.text);
                previousInput.Add(inputField.text);
                previousInputIndex = previousInput.Count;
                inputField.text = string.Empty;
                inputField.ActivateInputField();
            }
            else
            {
                consoleOutput.text += $"<color=red>Command not found: <noparse>{inputField.text}</noparse></color>\n";
                inputField.text = string.Empty;
                inputField.ActivateInputField();
            }
        }
    }

    private void NavigateHistory(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        if (value > 0) // Up
        {
            if (previousInputIndex > 0)
            {
                previousInputIndex--;
                inputField.text = previousInput[previousInputIndex];
            }
        }
        else if (value < 0) // Down
        {
            if (previousInputIndex < previousInput.Count - 1)
            {
                previousInputIndex++;
                inputField.text = previousInput[previousInputIndex];
            }
            else
            {
                previousInputIndex = previousInput.Count;
                inputField.text = string.Empty;
            }
        }
    }

    private void Update()
    {
        AutoCompleteCommand();
    }

    private void ToggleAutoComplete()
    {
        inputField.text = autocompleteField.text;
        inputField.caretPosition = inputField.text.Length;
    }

    private void AutoCompleteCommand()
    {
        if (inputField.text.Length > 1)
        {
            string currentInput = inputField.text;
            foreach (var command in CommandRegistry.GetAllCommands())
            {
                if (command.StartsWith(currentInput, System.StringComparison.OrdinalIgnoreCase))
                {
                    autocompleteField.text = command;
                    return;
                }
            }
            autocompleteField.text = string.Empty;
        }
        else
        {
            autocompleteField.text = string.Empty;
        }
    }
}
