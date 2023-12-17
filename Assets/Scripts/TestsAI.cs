using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestsAI : MonoBehaviour
{
    public TMP_Text textField;
    public TMP_InputField inputField;
    public Button okButton;

    private OpenAIAPI api;
    private List<ChatMessage> messages;

    void Start()
    {
        if (Constants.api_key.Length > 1)
        {
            api = new OpenAIAPI(new APIAuthentication(Constants.api_key));
        }
        else
        {
            //StartCoroutine(GetKeyFromServer()); //pull from github
        }
        StartConversation();
        okButton.onClick.AddListener(() => WriteStory());
        Debug.Log("start !");
    }

    private void StartConversation()
    {
        messages = new List<ChatMessage> {
            new ChatMessage(ChatMessageRole.System, "You are a detective who recreates the crime after the crime scene is described to you.  Your answers are short and on point."),
            //examples
            new ChatMessage(ChatMessageRole.User, "Old man ring on his finger, no signs of violent death, no signs of a break-in."),
            new ChatMessage(ChatMessageRole.System, "Probably poisoned by his wife."),
            new ChatMessage(ChatMessageRole.User, "The car is completely burned, no license plate, two bodies buried nearby. Traces of another car tires are leading from the burned car to the highway."),
            new ChatMessage(ChatMessageRole.System, "Drug deal, witnesses killed and buried, another car took over the goods.")
        };

        inputField.text = "";
        string startString = "Describe the scene.";
        textField.text = startString;
    }

    // Update is called once per frame
    private async void WriteStory()
    {
        if (inputField.text.Length < 1)
        {
            return;
        }

        // Disable the OK button
        okButton.enabled = false;

        // Fill the user message from the input field
        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;
        userMessage.Content = inputField.text;
        if (userMessage.Content.Length > 100)
        {
            // Limit messages to 100 characters
            userMessage.Content = userMessage.Content.Substring(0, 100);
        }
        Debug.Log(string.Format("{0}: {1}", userMessage.Role, userMessage.Content));

        // Add the message to the list
        messages.Add(userMessage);

        // Update the text field with the user message
        textField.text = string.Format("You: {0}", userMessage.Content);

        // Clear the input field
        inputField.text = "";

        // Send the entire chat to OpenAI to get the next message
        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.9,
            MaxTokens = 50,
            Messages = messages
        });

        // Get the response message
        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.Content = chatResult.Choices[0].Message.Content;
        Debug.Log(string.Format("{0}: {1}", responseMessage.Role, responseMessage.Content));

        // Add the response to the list of messages
        messages.Add(responseMessage);

        // Update the text field with the response
        textField.text = string.Format("You: {0}\n\nDetective: {1}", userMessage.Content, responseMessage.Content);

        // Re-enable the OK button
        okButton.enabled = true;
    }
}
