using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GPTChatAPIAccess : MonoBehaviour
{


    private string OPENAI_API_KEY;
    [SerializeField] private string OPENAI_API_KEY_Default;
    [SerializeField] private TMP_InputField input_OPENAI_API_KEY;


    private const string openaiEndpoint = "https://api.openai.com/v1/chat/completions";

    private void Start()
    {
        input_OPENAI_API_KEY.contentType = TMP_InputField.ContentType.Password;
        if (OPENAI_API_KEY_Default != null ) input_OPENAI_API_KEY.text = OPENAI_API_KEY_Default;
    }

    public void ApplyKey()
    {
        OPENAI_API_KEY = input_OPENAI_API_KEY.text;
    }

    public void SendPrompt(ChatPrompt promptObject, int correlationId, Action<string, int> callback, Action<string, int> errorCallback)
    {
        string jsonPayload = JsonConvert.SerializeObject(promptObject);
        Debug.Log($"jsonPayload={jsonPayload}");

        UnityWebRequest request = webRequest(jsonPayload);

        StartCoroutine(SendChatRequest(request, correlationId, callback, errorCallback));
    }

    public static ChatPrompt CreateRequest(List<Message> messages, double temperature, string gptModel)
    {
        var promptObject = new ChatPrompt
        {
            model = gptModel,
            messages = messages,
            temperature = temperature,
            max_tokens = 256,
            top_p = 1,
            frequency_penalty = 0,
            presence_penalty = 0,
            stop = new List<string> { "\n" }
        };

        return promptObject;
    }

    private UnityWebRequest webRequest(string jsonPayload)
    {
        UnityWebRequest request = new UnityWebRequest(openaiEndpoint, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + OPENAI_API_KEY);

        return request;
    }

    private IEnumerator SendChatRequest(UnityWebRequest request, int correlationId, Action<string, int> callback, Action<string, int> errorCallback)
    {
        Debug.Log("Sending request. Awaiting response...");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.downloadHandler.text);

            string responseText = request.downloadHandler.text;

            // Parse the JSON string
            JObject responseObject = JObject.Parse(responseText);

            // Extract the message
            string errorMessage = responseObject["error"]["message"].ToString();

            // Now you can use errorMessage to show it to the user
            errorCallback.Invoke("Message from OpenAI API: " + errorMessage, correlationId);
            Debug.Log(errorMessage);
        }

        else
        {
            string jsonStringResponse = request.downloadHandler.text;
            try
            {
                ChatPromptResponse parsedResponse = ChatGPTResponseParser.parse(jsonStringResponse);
                string content = parsedResponse.choices[0].message.content;
                callback(content, correlationId);
            }
            catch (JsonReaderException e)
            {
                errorCallback.Invoke(e.Message, correlationId);
                Debug.LogError($"Error: {e} Full response being parsed: {jsonStringResponse}");
            }
        }
    }

}
