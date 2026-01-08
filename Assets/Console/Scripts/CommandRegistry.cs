using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class CommandRegistry
{
    private static readonly Dictionary<NewCommandSO, Delegate> registry = new();

    public static void Register(NewCommandSO command, Action action)
        => registry[command] = action;

    public static void RegisterParams<T>(NewCommandSO command, Action<T> action)
        => registry[command] = action;

    public static void RegisterParams<T1, T2>(NewCommandSO command, Action<T1, T2> action)
        => registry[command] = action;

    public static bool Execute(string input)
    {
        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0) return false;

        foreach (var pair in registry)
        {
            if (!pair.Key.commandName.Equals(parts[0], StringComparison.OrdinalIgnoreCase))
                continue;

            var del = pair.Value;

            // 0 params
            if (del is Action a0)
            {
                if (parts.Length != 1)
                {
                    Debug.LogWarning($"Incorrect number of parameters for command: {parts[0]} (expected 0)");
                    return false;
                }
                a0();
                return true;
            }

            // 1 param
            if (del is Action<float> af)
            {
                if (parts.Length != 2)
                {
                    Debug.LogWarning($"Incorrect number of parameters for command: {parts[0]} (expected 1)");
                    return false;
                }
                if (!TryParse(parts[1], out float v))
                {
                    Debug.LogWarning($"Parameter conversion failed for command: {parts[0]} (arg0='{parts[1]}' expected float)");
                    return false;
                }
                af(v);
                return true;
            }

            if (del is Action<int> ai)
            {
                if (parts.Length != 2)
                {
                    Debug.LogWarning($"Incorrect number of parameters for command: {parts[0]} (expected 1)");
                    return false;
                }
                if (!TryParse(parts[1], out int v))
                {
                    Debug.LogWarning($"Parameter conversion failed for command: {parts[0]} (arg0='{parts[1]}' expected int)");
                    return false;
                }
                ai(v);
                return true;
            }

            if (del is Action<bool> ab)
            {
                if (parts.Length != 2)
                {
                    Debug.LogWarning($"Incorrect number of parameters for command: {parts[0]} (expected 1)");
                    return false;
                }
                if (!TryParse(parts[1], out bool v))
                {
                    Debug.LogWarning($"Parameter conversion failed for command: {parts[0]} (arg0='{parts[1]}' expected bool)");
                    return false;
                }
                ab(v);
                return true;
            }

            if (del is Action<string> aS)
            {
                if (parts.Length != 2)
                {
                    Debug.LogWarning($"Incorrect number of parameters for command: {parts[0]} (expected 1)");
                    return false;
                }
                aS(parts[1]);
                return true;
            }

            // 2 params (ejemplo: float,int)
            if (del is Action<float, int> aFI)
            {
                if (parts.Length != 3)
                {
                    Debug.LogWarning($"Incorrect number of parameters for command: {parts[0]} (expected 2)");
                    return false;
                }
                if (!TryParse(parts[1], out float f) || !TryParse(parts[2], out int n))
                {
                    Debug.LogWarning($"Parameter conversion failed for command: {parts[0]} (expected float int)");
                    return false;
                }
                aFI(f, n);
                return true;
            }

            Debug.LogWarning($"Command registered but unsupported signature: {parts[0]} ({del.GetType().Name})");
            return false;
        }

        Debug.LogWarning($"Command not found: {parts[0]}");
        return false;
    }

    private static bool TryParse<T>(string s, out T value)
    {
        value = default;

        if (typeof(T) == typeof(float))
        {
            var ok = float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var v);
            value = (T)(object)v;
            return ok;
        }
        if (typeof(T) == typeof(int))
        {
            var ok = int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var v);
            value = (T)(object)v;
            return ok;
        }
        if (typeof(T) == typeof(bool))
        {
            var ok = bool.TryParse(s, out var v);
            value = (T)(object)v;
            return ok;
        }
        if (typeof(T) == typeof(string))
        {
            value = (T)(object)s;
            return true;
        }

        try
        {
            value = (T)Convert.ChangeType(s, typeof(T), CultureInfo.InvariantCulture);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsRegistered(string commandName)
    {
        foreach (var pair in registry)
        {
            if (pair.Key.commandName.Equals(commandName, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }

    public static List<NewCommandSO> GetAllCommands()
    {
        List<NewCommandSO> commands = new List<NewCommandSO>();
        foreach (var pair in registry)
        {
            commands.Add(pair.Key);
        }
        return commands;
    }
}
