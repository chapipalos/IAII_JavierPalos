using System;
using System.Collections.Generic;
using UnityEngine;

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

    public override string ToString()
    {
        string res = $"{commandName} - {description}";
        if(parameters != null && parameters.Count > 0)
        {
            res += $"\n\tParams: \t[{string.Join("]\n\t\t\t[", parameters)}]";
        }
        return res + "\n";
    }
}

