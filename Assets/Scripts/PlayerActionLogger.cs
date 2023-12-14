using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerActionLogger : MonoBehaviour
{
    public static PlayerActionLogger Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LogAction(string action, InputAction.CallbackContext context)
    {
        string timestamp = System.DateTime.Now.ToString("dd HH:mm:ss");
        string logMessage = $"{timestamp} - {action} - {context}";

        // Log the message to a file or display it in the console
        Debug.Log(logMessage);
    }
}
