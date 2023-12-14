using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NPC_Persona : MonoBehaviour
{
    private OpenAIAPI api;
    private List<ChatMessage> messages;

    public TMP_Text textField;
    public TMP_InputField inputField;
    public bool gettingResponse = false;
    public Button EndConversation;

    private NPCMemory historyKeeper;
    private bool firstEncounter;

    private string personalityProfile;
    public string WhereAreWe;

    public List<string> CharacterNames; //Alice, Kathie, MainCharacter, Bob, etc. include different versions of the name
    public string MyName;

    public DynamicRelationships DR;

    string PlayerName;

    void Start()
    {
        if (Constants.api_key.Length > 1)
        {
            api = new OpenAIAPI(new APIAuthentication(Constants.api_key));
        }
        else
        {
            api = new OpenAIAPI(new APIAuthentication("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"));
        }

        // api = new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User));
        messages = new List<ChatMessage>();

        historyKeeper = GetComponent<NPCMemory>();
        firstEncounter = !historyKeeper.FileExists;

        MyName = historyKeeper.Name;

        personalityProfile = LoadInfo(Application.streamingAssetsPath + "/" + MyName + "/persona.txt");

    }

    private string LoadInfo(string path)
    {
        string persona = "";

        if (File.Exists(path))
        {
            string data = File.ReadAllText(path);
            persona = data.ToString();
        }
        else
        {
            Debug.LogWarning("Personality file not found.");
        }

        return persona;
    }

    public string LoadPersonality()
    {
        return personalityProfile;
    }

    public void StartDialog()
    {
        Messenger.Broadcast("DialogueStarted");
        StartConversation();
        EndConversation.onClick.AddListener(() => FinishConversation());
    }


    private void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame && !gettingResponse)
        {
            gettingResponse = true;
            GetResponse();
        }

    }
    private void StartConversation()
    {
        if (Constants.PlayerName != "")
        {
            PlayerName = Constants.PlayerName;
        }
        else
        {
            PlayerName = "Remy";
        }

        if (firstEncounter)
        {
            messages = new List<ChatMessage> {
            new ChatMessage(ChatMessageRole.System, personalityProfile),
            };

            if (historyKeeper.Name == "Kathie")
            {
                messages.Add(new ChatMessage(ChatMessageRole.System, "you are meeting your cousin " + PlayerName + " at the train station. She just arrived and you are going to help her to get to your place."));
            }
        }
        else
        {
            messages = historyKeeper.LoadChatMessages();
        }

        messages.Add(new ChatMessage(ChatMessageRole.System, "if asked about what is the place you are at, it's " + WhereAreWe));
        messages.Add(new ChatMessage(ChatMessageRole.System, "your feelings/attitude towards your conversation partner on the scale from -10 to 10 where -10 is absolute hate and 10 is absolute love stands at " + DR.LoadAttitude()));

        textField.text = historyKeeper.startingString;
    }

    private async void GetResponse()
    {
        if (inputField == null || inputField.text.Length < 1)
        {
            gettingResponse = false;
            return;
        }

        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;
        userMessage.Content = inputField.text;
        userMessage.Name = PlayerName;

        //messages.Add(new ChatMessage(ChatMessageRole.System, LoadInfoAboutPerson(userMessage.Content)));

        messages.Add(userMessage);
        inputField.text = "";

        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo0301,
            Temperature = 1,
            MaxTokens = 200,
            Messages = messages
        });

        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.Content = chatResult.Choices[0].Message.Content;
        responseMessage.Name = MyName;

        messages.Add(responseMessage);

        textField.text = responseMessage.Content;

        

        gettingResponse = false;
    }

    private void FinishConversation()
    {
        EndConversation.onClick.RemoveAllListeners();

        Debug.Log("finish clicked");
        historyKeeper.SaveChatMessages(messages); //TODO instead of rewriting, add 
        Messenger.Broadcast("DialogueFinished");
        gettingResponse = false;
        DR.EvaluateConversationImpact();
    }

    IEnumerator GetKeyFromServer()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://raw.githubusercontent.com/maria-shchurova/maria-shchurova.github.io/main/src/assets/0P3NA1K3Y.txt");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            api = new OpenAIAPI(new APIAuthentication(www.downloadHandler.text));
        }
    }
}
