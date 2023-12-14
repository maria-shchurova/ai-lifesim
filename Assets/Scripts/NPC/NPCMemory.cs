using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using OpenAI_API;
using OpenAI_API.Chat;

public class NPCMemory : MonoBehaviour
{
    [SerializeField]
    string name;
    [SerializeField]
    public string startingString;
    [SerializeField]
    public string codeName;

    string historyFilePath;

    public string Name
    {
        get { return name; }
    }

    private void Start()
    {
        historyFilePath = Application.streamingAssetsPath + "/" + Name + "/chatHistory.json";
    }
    public bool FileExists
    {
        get { return File.Exists(historyFilePath); }
    }

    public void SaveChatMessages(List<ChatMessage> chatMessages)
    {
        List<ChatMessage> previousHistory = LoadChatMessages();
        
        foreach(ChatMessage message in chatMessages)
        {
            previousHistory.Add(message); // adding new to old
        }
        var updatedHistory = previousHistory;

        string jsonContent = JsonConvert.SerializeObject(updatedHistory, Formatting.Indented);
        File.WriteAllText(historyFilePath, jsonContent);
        Debug.Log("Chat messages saved to file.");
    }

    public List<ChatMessage> LoadChatMessages()
    {
        List<ChatMessage> chatMessages = new List<ChatMessage>();

        if (File.Exists(historyFilePath))
        {
            string jsonContent = File.ReadAllText(historyFilePath);
            chatMessages = JsonConvert.DeserializeObject<List<ChatMessage>>(jsonContent);
        }
        else
        {
            Debug.LogWarning("Chat messages file not found.");
        }

        return chatMessages;
    }

    public string LoadChatMessagesAsText()
    {
        if (File.Exists(historyFilePath))
        {
            return File.ReadAllText(historyFilePath);
        }
        else
        {
            Debug.LogWarning("Chat messages file not found.");
        }

        return "";
    }
}



