using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameObject ResumeButton;
    private GameObject MenuButton;
    private GameObject RestartButton;
    private GameObject QuitButton;

    void Start()
    {
        ResumeButton = GameObject.Find("ResumeButton");
        MenuButton = GameObject.Find("MenuButton");
        RestartButton = GameObject.Find("RestartButton");
        QuitButton = GameObject.Find("QuitButton");


        ResumeButton.GetComponent<Button>().onClick.AddListener(Resume);
        MenuButton.GetComponent<Button>().onClick.AddListener(ToMenu);
        RestartButton.GetComponent<Button>().onClick.AddListener(Restart);
        QuitButton.GetComponent<Button>().onClick.AddListener(Quit);
    }

    void Resume()
    {
        Messenger.Broadcast("ResumeGame");
    }

    void ToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Messenger.Broadcast("ResumeGame");
    }

    void Quit()
    {
        Time.timeScale = 1;
        Application.Quit();
    }

}