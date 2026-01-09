using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConsoleSingleton : MonoBehaviour
{
    public static ConsoleSingleton Instance { get; private set; }

    public LifeimeController m_LifetimeController;

    public InputActionReference m_ToggleConsoleAction;

    public TMP_Text m_ConsoleOutput;
    public TMP_InputField m_InputField;
    public TMP_InputField m_AutocompleteField;

    public string m_Error;

    public bool m_ToggleConsole;

    public GameObject console;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        Instance.m_ToggleConsoleAction.action.Enable();
        Instance.m_ToggleConsoleAction.action.performed += ctx => ToggleConsole();
        Instance.console.SetActive(false);
    }

    public void ToggleConsole()
    {
        Instance.m_ToggleConsole = !Instance.m_ToggleConsole;
        Instance.console.SetActive(Instance.m_ToggleConsole);
        Instance.ShowConsole();
    }

    public void ShowConsole()
    {
        if (Instance.m_ToggleConsole)
        {
            Instance.m_InputField.ActivateInputField();
        }
        else
        {
            Instance.m_InputField.DeactivateInputField();
        }
    }
}
