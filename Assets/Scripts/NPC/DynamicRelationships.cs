using UnityEngine;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using System.Collections.Generic;

public class DynamicRelationships : MonoBehaviour
{
    protected OpenAIAPI api;
    protected List<ChatMessage> messages;

    [Range (-10, 10)]
    protected int attitudeScale; 
    protected NPCMemory historyKeeper;

    void Start()
    {
        if (Constants.api_key.Length > 1)
        {
            api = new OpenAIAPI(new APIAuthentication(Constants.api_key));
        }
        else
        {
            api = new OpenAIAPI(new APIAuthentication("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"));
            //StartCoroutine(GetKeyFromServer()); //pull from github
        }
        messages = new List<ChatMessage>();
        historyKeeper = GetComponent<NPCMemory>();
        attitudeScale = LoadAttitude();
    }

    public int LoadAttitude()
    {
        if(PlayerPrefs.HasKey(historyKeeper.Name + "attitudeScale"))
            return PlayerPrefs.GetInt(historyKeeper.Name + "attitudeScale");
        else
            return 0;
    }

    public void SaveAttitude(float value)
    {
        PlayerPrefs.SetFloat(historyKeeper.Name + "attitudeScale", value);
    }

    public virtual async void EvaluateConversationImpact()
    {
        Debug.Log("REQUEST");
        messages.Add(new ChatMessage(
            ChatMessageRole.System,
            historyKeeper.LoadChatMessagesAsText() +
            String.Format("Based on the provided chat history log, on the scale from -10 to 10, how much does {0} match {1}'s vibe?", Constants.PlayerName, historyKeeper.Name)+
            "Please answer with the result number as a single number and nothing else."
            ));

        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest() //too many tokens??
        {
            Model = Model.ChatGPTTurbo0301,
            Temperature = 0.5,
            MaxTokens = 1,
            Messages = messages
        });

        var s = chatResult.Choices[0].Message.Content;
        Debug.Log("chatResult.Choices[0].Message.Content " + s);

        attitudeScale += int.Parse(s); 
        Debug.Log("FeelingsTowardsPlayer changed to " + attitudeScale);
        SaveAttitude(attitudeScale);
    }
}
