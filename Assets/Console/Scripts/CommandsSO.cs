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

    public override string ToString()
    {
        return $"{commandName}: {description}";
    }
}

