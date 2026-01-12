using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[RequireComponent(typeof(PlayerInput))]
public class CommandsController : MonoBehaviour
{
    private List<string> m_PreviousInput = new List<string>();
    private int m_PreviousInputIndex = -1;

    // Input Actions
    public InputActionReference m_ToggleConsoleAction;
    public InputActionReference m_HistorialAction;
    public InputActionReference m_AutocompleteAction;

    private void Awake()
    {
        m_ToggleConsoleAction.action.Enable();
        m_ToggleConsoleAction.action.performed += ctx => ToggleActionConsole();

        m_HistorialAction.action.Enable();
        m_HistorialAction.action.performed += ctx => NavigateHistory(ctx);

        m_AutocompleteAction.action.Enable();
        m_AutocompleteAction.action.performed += ctx => ToggleAutoComplete();
    }

    private void ToggleActionConsole()
    {
        if (!string.IsNullOrWhiteSpace(ConsoleSingleton.m_Instance.m_InputField.text))
        {
            var parts = ConsoleSingleton.m_Instance.m_InputField.text.Split(' ');
            if (CommandRegistry.IsRegistered(parts[0]))
            {
                ConsoleSingleton.m_Instance.m_ConsoleOutput.text += $"\n -<color=green><noparse>{parts[0]}</noparse></color>";

                for (int i = 1; i < parts.Length; i++)
                {
                    ConsoleSingleton.m_Instance.m_ConsoleOutput.text += $"  <color=yellow><noparse>{parts[i]}</noparse></color>";
                }

                ConsoleSingleton.m_Instance.m_ConsoleOutput.text += $"\n";


                if (CommandRegistry.Execute(ConsoleSingleton.m_Instance.m_InputField.text))
                {
                    m_PreviousInput.Add(ConsoleSingleton.m_Instance.m_InputField.text);
                    m_PreviousInputIndex = m_PreviousInput.Count;
                }
                else
                {
                    ConsoleSingleton.m_Instance.m_ConsoleOutput.text += $"\n<color=red>{ConsoleSingleton.m_Instance.m_Error}</color>\n";
                }
                ConsoleSingleton.m_Instance.m_InputField.text = string.Empty;
                ConsoleSingleton.m_Instance.m_InputField.ActivateInputField();
            }
            else
            {
                ConsoleSingleton.m_Instance.m_ConsoleOutput.text += $"\n<color=red>Command not found: <noparse>{ConsoleSingleton.m_Instance.m_InputField.text}</noparse></color>\n";
                ConsoleSingleton.m_Instance.m_InputField.text = string.Empty;
                ConsoleSingleton.m_Instance.m_InputField.ActivateInputField();
            }
        }
    }

    private void NavigateHistory(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        if (value > 0)
        {
            if (m_PreviousInputIndex > 0)
            {
                m_PreviousInputIndex--;
                ConsoleSingleton.m_Instance.m_InputField.text = m_PreviousInput[m_PreviousInputIndex];
            }
        }
        else if (value < 0)
        {
            if (m_PreviousInputIndex < m_PreviousInput.Count - 1)
            {
                m_PreviousInputIndex++;
                ConsoleSingleton.m_Instance.m_InputField.text = m_PreviousInput[m_PreviousInputIndex];
            }
            else
            {
                m_PreviousInputIndex = m_PreviousInput.Count;
                ConsoleSingleton.m_Instance.m_InputField.text = string.Empty;
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
        ConsoleSingleton.m_Instance.m_InputField.text = ConsoleSingleton.m_Instance.m_AutocompleteField.text;
        ConsoleSingleton.m_Instance.m_InputField.caretPosition = ConsoleSingleton.m_Instance.m_InputField.text.Length;
    }

    private void AutoCompleteCommand()
    {
        if (ConsoleSingleton.m_Instance.m_InputField.text.Length > 1)
        {
            string currentInput = ConsoleSingleton.m_Instance.m_InputField.text;
            foreach (var command in CommandRegistry.GetAllCommands())
            {
                if (command.m_CommandName.StartsWith(currentInput))
                {
                    ConsoleSingleton.m_Instance.m_AutocompleteField.text = command.m_CommandName;
                    return;
                }
            }
            ConsoleSingleton.m_Instance.m_AutocompleteField.text = string.Empty;
        }
        else
        {
            ConsoleSingleton.m_Instance.m_AutocompleteField.text = string.Empty;
        }
    }

    private void CheckShortcuts()
    {
        foreach (var command in CommandRegistry.GetAllCommands())
        {
            var keys = command.m_ShortCuts;
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

            ConsoleSingleton.m_Instance.m_ConsoleOutput.text += $"\n -<color=green><noparse>{command.m_CommandName}</noparse></color>\n";
            m_PreviousInput.Add(command.m_CommandName);
            if(CommandRegistry.HasParameters(command.m_CommandName))
            {
                ConsoleSingleton.m_Instance.m_ConsoleOutput.text += $"\n<color=red>Command '{command.m_CommandName}' requires parameters and cannot be executed via shortcut.</color>\n";
            }
            else
            { 
                CommandRegistry.Execute(command.m_CommandName);
            }
        }
    }
}
