using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCommands : MonoBehaviour
{
    public CommandsSO commandsSO;

    // ----------------------------------------------------------
    // ALL PARAMETERS AND VARIABLES FOR COMMANDS TO USE
    // ----------------------------------------------------------

    public TMPro.TextMeshProUGUI consoleOutput;

    public LifeimeController lifetimeController;

    void Awake()
    {
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
                CommandRegistry.Register(command, ClearConsole);
                break;

            case "help":
                CommandRegistry.Register(command, Help);
                break;
            case "des":
                CommandRegistry.RegisterParams<float>(command, t => DestroyObject(t));
                break;
            case "scn":
                CommandRegistry.Register(command, LoadScene);
                break;
        }
    }

    void ClearConsole()
    {
        consoleOutput.text = string.Empty;
    }

    void Help()
    {
        foreach (var command in commandsSO.allCommands)
        {
            consoleOutput.text += $"<color=white>{command}</color>\n";
        }
    }

    void DestroyObject(float time)
    {
        if (lifetimeController != null)
            lifetimeController.StartLifetime(time);
    }

    void LoadScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (sceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
