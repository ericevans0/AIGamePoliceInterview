using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GPTUserDirectPrompt : MonoBehaviour
{
    [SerializeField] private string OPENAI_API_KEY;
    [SerializeField] private TMP_InputField promptInputField;
    [SerializeField] private TMP_Text responseDisplay;

    private const string openaiEndpoint = "https://api.openai.com/v1/chat/completions";

    private void Start()
    {
        Debug.Log("GPTUserDirectPrompt.Start()");
        responseDisplay.text = "";
    }

    public void SendPrompt()
    {
        string prompt = promptInputField.text;
        Debug.Log($"Prompt: {prompt}");
        StartCoroutine(SendChatRequest(prompt));
    }

    private IEnumerator SendChatRequest(string prompt)
    {
        string jsonPayload = "{\"model\": \"gpt-3.5-turbo\", \"messages\": [{\"role\": \"user\", \"content\": \"" + prompt + "\"}], \"temperature\": 0.7}";
//        string jsonPayload = "{\"model\": \"gpt-4\", \"messages\": [{\"role\": \"user\", \"content\": \"" + prompt + "\"}], \"temperature\": 0.7}";
        Debug.Log($"jsonPayload={jsonPayload}");
        UnityWebRequest request = new UnityWebRequest(openaiEndpoint, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + OPENAI_API_KEY);

        Debug.Log("Sending request. Awaiting response...");
        responseDisplay.text += "\n" + prompt;
        //responseDisplay.text = "Sending request. Awaiting response...";
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
            responseDisplay.text += "\n" + "Error: " + request.error;

        }
        else
        {
            string jsonStringResponse = request.downloadHandler.text;
            ChatPromptResponse parsedResponse = ChatGPTResponseParser.parse(jsonStringResponse);
            string response = parsedResponse.choices[0].message.content;
            Debug.Log("GPT ChatPromptResponse: " + response);

            responseDisplay.text += "\n" + response;
        }
    }
}
