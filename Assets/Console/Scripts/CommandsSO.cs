using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "CommandsSO", menuName = "Commands/Commands List")]
public class CommandsSO : ScriptableObject
{
    public List<NewCommandSO> m_AllCommands;
}

[CreateAssetMenu(fileName = "Command", menuName = "Commands/Command")]
public class NewCommandSO : ScriptableObject
{
    public string m_CommandName;
    public string m_Description;
    public List<string> m_Parameters;

    public List<Key> m_ShortCuts = new List<Key>();

    public override string ToString()
    {
        string res = $"{m_CommandName} - {m_Description}";
        if(m_Parameters != null && m_Parameters.Count > 0)
        {
            res += $"\n\tParams: \t[{string.Join("]\n\t\t\t[", m_Parameters)}]";
        }
        if (m_ShortCuts != null && m_ShortCuts.Count > 0)
        {
            res += $"\n\t[{string.Join("] + [", m_ShortCuts)}]";
        }
        return res + "\n";
    }
}

