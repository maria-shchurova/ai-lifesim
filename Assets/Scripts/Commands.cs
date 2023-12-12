using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using UnityEditor;
using System.Threading.Tasks;
using System.Threading;

public class Commands : MonoBehaviour
{
    private OpenAIAPI api;
    private List<ChatMessage> messages;

    public TMP_Text textField;
    public TMP_InputField inputField;
    public Button okButton;


    void Start()
    {
        api = new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User));
        messages = new List<ChatMessage>();
        okButton.onClick.AddListener(() => GenerateCode());
    }

    static string WrapPrompt(string input)
     => "Write a Unity script named 'Default' without commentaries and explanations. It must have a public static method 'Foo' for implementation of the following request:\n " + input;


    private async void GenerateCode()
    {
        if (inputField.text.Length < 1)
        {
            return;
        }

        okButton.enabled = false;

        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;
        userMessage.Content = WrapPrompt(inputField.text);

        Debug.Log(string.Format("{0}: {1}", userMessage.Role, userMessage.Content));

        messages.Add(userMessage);

        textField.text = string.Format("You: {0}", userMessage.Content);
        inputField.text = "";

        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.9,
            Messages = messages
        });

        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.Content = chatResult.Choices[0].Message.Content;

        messages.Add(responseMessage);

        // Update the text field with the response
        textField.text = string.Format("You: {0}\n\nChat: {1}", userMessage.Content, responseMessage.Content);
        Debug.Log("AI command script:" + responseMessage.Content);


        var assembly = CompilerExample.Compile(responseMessage.Content);
        var method = assembly.GetType("Default").GetMethod("Foo");
        var del = (Action)Delegate.CreateDelegate(typeof(Action), method);
        del.Invoke();

        // Re-enable the OK button
        okButton.enabled = true;
    }

}


