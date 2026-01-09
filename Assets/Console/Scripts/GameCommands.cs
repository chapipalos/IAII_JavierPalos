using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCommands : MonoBehaviour
{
    public CommandsSO commandsSO;

    void Awake()
    {
        CommandRegistry.Clear();
        foreach (var command in commandsSO.allCommands)
        {
            RegisterCommand(command);
        }
    }

    void RegisterCommand(NewCommandSO command)
    {
        switch (command.commandName.ToLower())
        {
            case "clear":
                CommandRegistry.Register(command, (Action)ClearConsole);
                break;

            case "help":
                CommandRegistry.Register(command, (Action)Help);
                break;
            case "des":
                CommandRegistry.Register(command, (Action<float>)DestroyObject);
                break;
            case "scn":
                CommandRegistry.Register(command, (Action)LoadScene);
                break;
        }
    }

    void ClearConsole()
    {
        ConsoleSingleton.Instance.m_ConsoleOutput.text = string.Empty;
    }

    void Help()
    {
        ConsoleSingleton.Instance.m_ConsoleOutput.text += "\n";
        foreach (var command in commandsSO.allCommands)
        {
            ConsoleSingleton.Instance.m_ConsoleOutput.text += $"<color=white>{command}</color>\n";
        }
    }

    void DestroyObject(float time)
    {
        LifeimeController lifeimeController = FindFirstObjectByType<LifeimeController>();
        if (lifeimeController != null)
            lifeimeController.StartLifetime(time);
    }

    void LoadScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (sceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
