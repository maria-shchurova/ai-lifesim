using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public TMP_InputField nameInput;
    public Button submitName;

    public GameObject IntroWindow;

    private string[] NPCnames = { "Kathie", "Alice", "Ernest", "Trenton" };


    void Start()
    {
        CleanSavedData();
        submitName.onClick.AddListener(() => SubmitName());
        Messenger.Broadcast("Pause");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CleanSavedData()
    {
        foreach (string s in NPCnames)
        {
            string path = Application.streamingAssetsPath + "/" + s + "/chatHistory.json";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        PlayerPrefs.DeleteAll();
    }

    void SubmitName()
    {
        if(nameInput.text.Length < 1)
        {
            Constants.PlayerName = "Remy";
        }
        else
        {
            Constants.PlayerName = nameInput.text;
        }

        PlayerPrefs.SetString("PlayerName", Constants.PlayerName);

        Messenger.Broadcast("ResumeGame"); 
        IntroWindow.SetActive(false);
    }



    public void StartGame()
    {
        SceneManager.LoadScene("KathiesHouse");
    }
}
