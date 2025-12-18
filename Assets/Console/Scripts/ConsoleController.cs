using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class ConsoleController : MonoBehaviour
{
    public InputActionReference m_ToggleConsoleAction;

    public TMP_InputField m_InputField;

    private bool m_ToggleConsole;

    void Start()
    {
        m_ToggleConsole = false;
        ShowConsole(m_ToggleConsole);
        m_ToggleConsoleAction.action.Enable();
        m_ToggleConsoleAction.action.performed += ctx => ToggleConsole();
    }

    private void ToggleConsole()
    {
        m_ToggleConsole = !m_ToggleConsole;
        ShowConsole(m_ToggleConsole);
    }

    private void ShowConsole(bool show)
    {
        gameObject.SetActive(show);
        if (show)
        {
            m_InputField.ActivateInputField();
        }
        else
        {
            m_InputField.DeactivateInputField();
            m_InputField.text = string.Empty;
        }
    }
}
