using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConsoleSingleton : MonoBehaviour
{
    public static ConsoleSingleton m_Instance { get; private set; }

    public InputActionReference m_ToggleConsoleAction;

    public TMP_Text m_ConsoleOutput;
    public TMP_InputField m_InputField;
    public TMP_InputField m_AutocompleteField;

    public string m_Error;

    private bool m_ToggleConsole;

    public GameObject m_Console;

    private void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        m_Instance.m_ToggleConsoleAction.action.Enable();
        m_Instance.m_ToggleConsoleAction.action.performed += ctx => ToggleConsole();
        m_Instance.m_Console.SetActive(false);
    }

    public void ToggleConsole()
    {
        m_Instance.m_ToggleConsole = !m_Instance.m_ToggleConsole;
        m_Instance.m_Console.SetActive(m_Instance.m_ToggleConsole);
        m_Instance.ShowConsole();
    }

    public void ShowConsole()
    {
        if (m_Instance.m_ToggleConsole)
        {
            m_Instance.m_InputField.ActivateInputField();
        }
        else
        {
            m_Instance.m_InputField.DeactivateInputField();
        }
    }
}
