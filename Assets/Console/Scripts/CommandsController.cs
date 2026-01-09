using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[RequireComponent(typeof(PlayerInput))]
public class CommandsController : MonoBehaviour
{
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
        if (!string.IsNullOrWhiteSpace(ConsoleSingleton.Instance.m_InputField.text))
        {
            var parts = ConsoleSingleton.Instance.m_InputField.text.Split(' ');
            if (CommandRegistry.IsRegistered(parts[0]))
            {
                ConsoleSingleton.Instance.m_ConsoleOutput.text += $"\n -<color=green><noparse>{parts[0]}</noparse></color>";

                for (int i = 1; i < parts.Length; i++)
                {
                    ConsoleSingleton.Instance.m_ConsoleOutput.text += $"  <color=yellow><noparse>{parts[i]}</noparse></color>";
                }

                ConsoleSingleton.Instance.m_ConsoleOutput.text += $"\n";


                if (CommandRegistry.Execute(ConsoleSingleton.Instance.m_InputField.text))
                {
                    previousInput.Add(ConsoleSingleton.Instance.m_InputField.text);
                    previousInputIndex = previousInput.Count;
                }
                else
                {
                    ConsoleSingleton.Instance.m_ConsoleOutput.text += $"\n<color=red>{ConsoleSingleton.Instance.m_Error}</color>\n";
                }
                ConsoleSingleton.Instance.m_InputField.text = string.Empty;
                ConsoleSingleton.Instance.m_InputField.ActivateInputField();
            }
            else
            {
                ConsoleSingleton.Instance.m_ConsoleOutput.text += $"\n<color=red>Command not found: <noparse>{ConsoleSingleton.Instance.m_InputField.text}</noparse></color>\n";
                ConsoleSingleton.Instance.m_InputField.text = string.Empty;
                ConsoleSingleton.Instance.m_InputField.ActivateInputField();
            }
        }
    }

    private void NavigateHistory(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        if (value > 0)
        {
            if (previousInputIndex > 0)
            {
                previousInputIndex--;
                ConsoleSingleton.Instance.m_InputField.text = previousInput[previousInputIndex];
            }
        }
        else if (value < 0)
        {
            if (previousInputIndex < previousInput.Count - 1)
            {
                previousInputIndex++;
                ConsoleSingleton.Instance.m_InputField.text = previousInput[previousInputIndex];
            }
            else
            {
                previousInputIndex = previousInput.Count;
                ConsoleSingleton.Instance.m_InputField.text = string.Empty;
            }
        }
    }

    private void Update()
    {
        AutoCompleteCommand();
        CheckShortcuts();
    }

    private void ToggleAutoComplete()
    {
        ConsoleSingleton.Instance.m_InputField.text = ConsoleSingleton.Instance.m_AutocompleteField.text;
        ConsoleSingleton.Instance.m_InputField.caretPosition = ConsoleSingleton.Instance.m_InputField.text.Length;
    }

    private void AutoCompleteCommand()
    {
        if (ConsoleSingleton.Instance.m_InputField.text.Length > 1)
        {
            string currentInput = ConsoleSingleton.Instance.m_InputField.text;
            foreach (var command in CommandRegistry.GetAllCommands())
            {
                if (command.commandName.StartsWith(currentInput))
                {
                    ConsoleSingleton.Instance.m_AutocompleteField.text = command.commandName;
                    return;
                }
            }
            ConsoleSingleton.Instance.m_AutocompleteField.text = string.Empty;
        }
        else
        {
            ConsoleSingleton.Instance.m_AutocompleteField.text = string.Empty;
        }
    }

    private void CheckShortcuts()
    {
        foreach (var command in CommandRegistry.GetAllCommands())
        {
            var keys = command.shortCuts;
            if (keys == null || keys.Count == 0) continue;

            bool allHeld = true;
            bool anyPressedThisFrame = false;

            for (int k = 0; k < keys.Count; k++)
            {
                var key = Keyboard.current[keys[k]];

                if (!key.isPressed)
                {
                    allHeld = false;
                    break;
                }

                if (key.wasPressedThisFrame)
                    anyPressedThisFrame = true;
            }

            if (!allHeld || !anyPressedThisFrame) continue;

            ConsoleSingleton.Instance.m_ConsoleOutput.text += $"\n -<color=green><noparse>{command.commandName}</noparse></color>\n";
            previousInput.Add(command.commandName);
            CommandRegistry.Execute(command.commandName);
        }
    }
}
