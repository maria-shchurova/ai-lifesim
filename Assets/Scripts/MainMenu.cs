using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private GameObject NewGame;
    private GameObject PlaySaved;
    private GameObject CreditsButton;
    private GameObject QuitButton;
    private GameObject AddYourKey;

    public TMP_InputField input;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.None;
        NewGame = GameObject.Find("NewGame");
        PlaySaved = GameObject.Find("PlaySaved");
        CreditsButton = GameObject.Find("CreditsButton");
        QuitButton = GameObject.Find("QuitButton");
        AddYourKey = GameObject.Find("AddYourKey");


        NewGame.GetComponent<Button>().onClick.AddListener(PlayNew);
        PlaySaved.GetComponent<Button>().onClick.AddListener(Continue);
        CreditsButton.GetComponent<Button>().onClick.AddListener(Credits);
        QuitButton.GetComponent<Button>().onClick.AddListener(Quit);
        AddYourKey.GetComponent<Button>().onClick.AddListener(AddUserKey);

        PlaySaved.GetComponent<Button>().enabled = (Constants.PlayerName.Length > 1);
    }

    void PlayNew()
    {
        SceneManager.LoadScene("Intro");
    }
    void Continue()
    {
        SceneManager.LoadScene("newCity");
    }

    void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    void AddUserKey()
    {
        if(input.text.Length > 1)
            Constants.api_key = input.text; //TODO check validity
    }

    void Quit()
    {
        Application.Quit();
    }
}