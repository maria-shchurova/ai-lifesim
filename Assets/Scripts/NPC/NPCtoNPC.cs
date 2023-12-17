using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Net.Http;

public class NPCtoNPC : MonoBehaviour
{
    [Range(2f, 6f)]
    public float DelayBetweenRequests;
    public float TalkDistance;
    public bool conversationGoing = false;

    private OpenAIAPI api;

    [SerializeField]
    private List<ChatMessage> messages;

    public NPC_Persona NPC1, NPC2;

    public TMP_Text NPC_dialog_textbox;

    public string WhatToTalkAbout;

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

        StartConversation();
    }


    void StartConversation()
    {
        messages.Add(new ChatMessage(ChatMessageRole.User, NPC1.LoadPersonality()));
        messages.Add(new ChatMessage(ChatMessageRole.User, NPC2.LoadPersonality()));

        messages.Add(new ChatMessage(ChatMessageRole.System, "This is a role-playing game"));
        messages.Add(new ChatMessage(ChatMessageRole.System, String.Format("{0} and {1} are talking about {2} ", NPC1.MyName, NPC2.MyName, WhatToTalkAbout)));
        messages.Add(new ChatMessage(ChatMessageRole.System, "Please provide the input in the following format:"));
        messages.Add(new ChatMessage(ChatMessageRole.System, "[Character Name]: [Prompt]"));
        messages.Add(new ChatMessage(ChatMessageRole.System, "Please avoid using quotation marks and keep each prompt limited to a single character response."));

        conversationGoing = true;
        Responce();
    }

    private async void Responce()
    {
        if (conversationGoing)
        {
            string lastMessage = messages[messages.Count - 1].Content.ToLower();

            if (lastMessage.Contains("see you") ||
               lastMessage.Contains("i'll be back") ||
               lastMessage.Contains("deal") ||
               lastMessage.Contains("you should come") ||
               lastMessage.Contains("let's see") ||
               lastMessage.Contains("okay") ||
               lastMessage.Contains("here for you"))
            {
                FinishTalking();
                Debug.Log("conversation finished");
            }

            var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
            {
                Model = Model.ChatGPTTurbo0301,
                Temperature = 0.9,
                MaxTokens = 50,
                Messages = messages
            });

            ChatMessage responseMessage = new ChatMessage();
            responseMessage.Role = chatResult.Choices[0].Message.Role;
            responseMessage.Content = chatResult.Choices[0].Message.Content;

            messages.Add(responseMessage);

            NPC_dialog_textbox.text = responseMessage.Name + responseMessage.Content;

            StartCoroutine(Delay());
        }
        else
            return;

    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(DelayBetweenRequests); //delay between responses - limited by 60 requests per minute
        Responce();    }

    void FinishTalking()
    {
        conversationGoing = false;
        StopCoroutine(Delay());
        NPC_dialog_textbox.text = "";
    }
}
