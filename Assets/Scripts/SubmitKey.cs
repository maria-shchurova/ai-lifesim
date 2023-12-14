using TMPro;
using UnityEngine;
using UnityEngine.UI;
using OpenAI_API;

public class SubmitKey : MonoBehaviour
{
    public Button Submit;
    public TMP_InputField KeyField;
    public Button StartButton;
    public TMP_Text Log;

    void Start()
    {
        Submit.onClick.AddListener(() => AddKey());
        StartButton.onClick.AddListener(() => Play());
    }

    bool IsApiKeyOk
     => !string.IsNullOrEmpty(Constants.api_key);

    void AddKey()
    {
        Constants.api_key = KeyField.text;
        if(IsApiKeyOk)
        {
            StartButton.gameObject.SetActive(true);
        }
        else
        {
            StartButton.gameObject.SetActive(false);
            Log.text = "API key is not valid.";
        }
    }

    private void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
