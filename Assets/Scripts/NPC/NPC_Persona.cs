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
using System.Threading.Tasks;

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

    public bool isTalking;

    //public DynamicRelationships DR;

    string PlayerName;

    void Start()
    {
        if (Constants.api_key.Length > 1)
        {
            api = new OpenAIAPI(new APIAuthentication(Constants.api_key));
        }
        else
        {
            api = new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User));
        }

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
        Messenger.Broadcast("DialogueMode");
        Messenger.Broadcast("DialogueStarted", this);
        StartConversation();
        EndConversation.onClick.AddListener(() => FinishConversation());
    }


    private void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame && !gettingResponse)
        {
            if (inputField == null || inputField.text.Length < 1)
            {
                gettingResponse = false;
                return;
            }

            StartCoroutine(GetResponseCoroutine());
        }

    }
    private void StartConversation()
    {
        isTalking = true;

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
       // messages.Add(new ChatMessage(ChatMessageRole.System, "your feelings/attitude towards your conversation partner on the scale from -10 to 10 where -10 is absolute hate and 10 is absolute love stands at " + DR.LoadAttitude()));

        textField.text = historyKeeper.startingString;
    }

    private IEnumerator GetResponseCoroutine()
    {
        gettingResponse = true;

        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;
        userMessage.Content = inputField.text;
        userMessage.Name = PlayerName;

        //messages.Add(new ChatMessage(ChatMessageRole.System, LoadInfoAboutPerson(userMessage.Content)));

        messages.Add(userMessage);
        inputField.text = "";

        TaskCompletionSource<ChatResult> taskCompletionSource = new TaskCompletionSource<ChatResult>();

        Task.Run(async () =>
        {
            var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
            {
                Model = Model.GPT4,
                Temperature = 1,
                MaxTokens = 200,
                Messages = messages
            });

            taskCompletionSource.SetResult(chatResult);
        });

        while (!taskCompletionSource.Task.IsCompleted)
        {
            yield return null;
        }

        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = taskCompletionSource.Task.Result.Choices[0].Message.Role;
        responseMessage.Content = taskCompletionSource.Task.Result.Choices[0].Message.Content;
        responseMessage.Name = MyName;

        messages.Add(responseMessage);

        textField.text = responseMessage.Content;        

        gettingResponse = false;
    }

    private void FinishConversation()
    {
        isTalking = false;

        EndConversation.onClick.RemoveAllListeners();

        Debug.Log("finish clicked");
        historyKeeper.SaveChatMessages(messages); //TODO instead of rewriting, add 
        Messenger.Broadcast("DialogueFinished");
        gettingResponse = false;
       // DR.EvaluateConversationImpact();
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
