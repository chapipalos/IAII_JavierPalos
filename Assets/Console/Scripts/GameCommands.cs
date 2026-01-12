using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCommands : MonoBehaviour
{
    public CommandsSO m_CommandsSO;

    void Awake()
    {
        CommandRegistry.Clear();
        foreach (var command in m_CommandsSO.m_AllCommands)
        {
            RegisterCommand(command);
        }
    }

    void RegisterCommand(NewCommandSO command)
    {
        switch (command.m_CommandName.ToLower())
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
            case "create":
                CommandRegistry.Register(command, (Action<int, float, float>)CreateSpheres);
                break;
        }
    }

    void ClearConsole()
    {
        ConsoleSingleton.m_Instance.m_ConsoleOutput.text = string.Empty;
    }

    void Help()
    {
        ConsoleSingleton.m_Instance.m_ConsoleOutput.text += "\n";
        foreach (var command in m_CommandsSO.m_AllCommands)
        {
            ConsoleSingleton.m_Instance.m_ConsoleOutput.text += $"<color=white>{command}</color>\n";
        }
    }

    void DestroyObject(float time)
    {
        TestComandsController testCommands = FindFirstObjectByType<TestComandsController>();
        if (testCommands != null)
            testCommands.StartLifetime(time);
    }

    void LoadScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (sceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }

    void CreateSpheres(int quantity, float minSpeed, float maxSpeed)
    {
        TestComandsController testCommands = FindFirstObjectByType<TestComandsController>();
        if (testCommands != null)
            testCommands.SpawnTestObject(quantity, minSpeed, maxSpeed);
    }
}
