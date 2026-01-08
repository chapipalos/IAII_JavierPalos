using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "CommandsSO", menuName = "Commands/Commands List")]
public class CommandsSO : ScriptableObject
{
    public List<NewCommandSO> allCommands;
}

[CreateAssetMenu(fileName = "Command", menuName = "Commands/Command")]
public class NewCommandSO : ScriptableObject
{
    public string commandName;
    public string description;
    public List<string> parameters;

    public List<Key> shortCuts = new List<Key>();

    public override string ToString()
    {
        string res = $"{commandName} - {description}";
        if(parameters != null && parameters.Count > 0)
        {
            res += $"\n\tParams: \t[{string.Join("]\n\t\t\t[", parameters)}]";
        }
        if (shortCuts != null && shortCuts.Count > 0)
        {
            res += $"\n\t[{string.Join("] + [", shortCuts)}]";
        }
        return res + "\n";
    }
}

