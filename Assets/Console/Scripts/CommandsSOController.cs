using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CommandsSOController", menuName = "Scriptable Objects/CommandsSO")]
public class CommandsSOController : ScriptableObject
{
    public List<CommandSO> m_AllCommands = new List<CommandSO>();
}

[Serializable]
public class CommandSO
{
    public string commandName;
    public List<ParameterSO> parameters = new List<ParameterSO>();
    public string description;
    public CommandEvent commandEvent;

    public override string ToString()
    {
        string paramsString = string.Join("; ", parameters);
        return $"{commandName}: ({paramsString}) - {description}";
    }
}

[Serializable]
public class ParameterSO
{
    public string parameterName;
    public string description;
    public override string ToString()
    {
        return $"({parameterName}: {description})";
    }
}

[Serializable]
public class CommandEvent : UnityEvent { }

