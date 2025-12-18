using System;
using System.Collections.Generic;
using UnityEngine;

public static class CommandRegistry
{
    private static readonly Dictionary<NewCommandSO, Action<string[]>> registry
        = new();

    public static void Register(NewCommandSO command, Action<string[]> action)
    {
        registry[command] = action;
    }

    public static bool Execute(string input)
    {
        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0) return false;

        foreach (var pair in registry)
        {
            if (pair.Key.commandName.Equals(parts[0], StringComparison.OrdinalIgnoreCase))
            {
                pair.Value.Invoke(parts.Length > 1 ? parts[1..] : Array.Empty<string>());
                return true;
            }
        }

        Debug.LogWarning($"Command not found: {parts[0]}");
        return false;
    }

    public static bool IsRegistered(string commandName)
    {
        foreach (var pair in registry)
        {
            if (pair.Key.commandName.Equals(commandName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }
}
