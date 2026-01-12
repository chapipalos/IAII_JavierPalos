using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using UnityEngine;

public static class CommandRegistry
{
    private static readonly Dictionary<NewCommandSO, Delegate> m_Registry = new();

    public static void Register(NewCommandSO command, Delegate del)
        => m_Registry[command] = del;

    public static void Clear() => m_Registry.Clear();

    public static bool Execute(string input)
    {
        var parts = input.Split(' ');
        if (parts.Length == 0) return false;

        var cmdName = parts[0];

        foreach (var pair in m_Registry)
        {
            if (!pair.Key.m_CommandName.Equals(cmdName))
                continue;

            var del = pair.Value;
            var paramInfos = del.Method.GetParameters();
            var expected = paramInfos.Length;
            var provided = parts.Length - 1;

            if (provided != expected)
            {
                ConsoleSingleton.m_Instance.m_Error =
                    $"Incorrect number of parameters for command: {cmdName} (expected {expected})";
                return false;
            }

            var args = new object[expected];

            for (int i = 0; i < expected; i++)
            {
                var raw = parts[i + 1];
                var targetType = paramInfos[i].ParameterType;

                if (!TryConvert(raw, targetType, out var converted))
                {
                    ConsoleSingleton.m_Instance.m_Error =
                        $"Parameter conversion failed for command: {cmdName} (arg{i}='{raw}' expected {targetType.Name})";
                    return false;
                }

                args[i] = converted;
            }

            try
            {
                del.DynamicInvoke(args);
                return true;
            }
            catch (TargetInvocationException tie)
            {
                var msg = tie.InnerException != null ? tie.InnerException.Message : tie.Message;
                ConsoleSingleton.m_Instance.m_Error = $"Command '{cmdName}' threw: {msg}";
                return false;
            }
            catch (Exception e)
            {
                ConsoleSingleton.m_Instance.m_Error = $"Failed to execute command '{cmdName}': {e.Message}";
                return false;
            }
        }

        ConsoleSingleton.m_Instance.m_Error = $"Command not found: {cmdName}";
        return false;
    }

    private static bool TryConvert(string s, Type targetType, out object value)
    {
        value = null;

        // strings directos
        if (targetType == typeof(string))
        {
            value = s;
            return true;
        }

        // enums
        if (targetType.IsEnum)
        {
            if (Enum.TryParse(targetType, s, ignoreCase: true, out var enumVal))
            {
                value = enumVal;
                return true;
            }
            return false;
        }

        // bool
        if (targetType == typeof(bool))
        {
            if (bool.TryParse(s, out var b))
            {
                value = b;
                return true;
            }
            return false;
        }

        // números con InvariantCulture (float, int, double, etc.)
        if (targetType == typeof(float))
        {
            if (float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var f))
            {
                value = f;
                return true;
            }
            return false;
        }

        if (targetType == typeof(int))
        {
            if (int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i))
            {
                value = i;
                return true;
            }
            return false;
        }

        try
        {
            var converter = TypeDescriptor.GetConverter(targetType);
            if (converter != null && converter.CanConvertFrom(typeof(string)))
            {
                value = converter.ConvertFrom(null, CultureInfo.InvariantCulture, s);
                return true;
            }
        }
        catch {}

        try
        {
            value = Convert.ChangeType(s, targetType, CultureInfo.InvariantCulture);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsRegistered(string commandName)
    {
        foreach (var pair in m_Registry)
        {
            if (pair.Key.m_CommandName.Equals(commandName))
                return true;
        }
        return false;
    }

    public static List<NewCommandSO> GetAllCommands()
    {
        var commands = new List<NewCommandSO>();
        foreach (var pair in m_Registry)
            commands.Add(pair.Key);
        return commands;
    }

    public static bool HasParameters(string commandName)
    {
        foreach (var pair in m_Registry)
        {
            if (pair.Key.m_CommandName.Equals(commandName))
            {
                var del = pair.Value;
                var paramInfos = del.Method.GetParameters();
                return paramInfos.Length > 0;
            }
        }
        return false;
    }
}
