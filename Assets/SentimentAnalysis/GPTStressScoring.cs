using System;
using System.Collections.Generic;
using UnityEngine;

public class GPTStressScoring : MonoBehaviour
{

    [SerializeField] private GPTChatAPIAccess apiAccess;
    [SerializeField] private LMSelection lmSelection;
    [SerializeField] private EditablePromptTemplate promptTemplate;
    private string currentPromptTemplate => promptTemplate.PromptTemplate;


    public void ScoreStress(string detectiveStatement, int correlationId, InterviewOrchestrator orchestrator)
    {
        Debug.Log($"Scoring stress for statement: {detectiveStatement}");

        string systemPrompt = currentPromptTemplate;

        Message tellGPTWhoItIs = new Message
        {
            role = "system",
            content = systemPrompt
        };

        Message describeDetectiveStatement = new Message
        {
            role = "user",
            content = detectiveStatement
        };

        List<Message> messages = new List<Message>()
        { 
            tellGPTWhoItIs,
            describeDetectiveStatement
        };

        ChatPrompt promptObject = GPTChatAPIAccess.CreateRequest(messages, lmSelection.Temperature, lmSelection.TargetLM);

        Action<string, int> callback = (response, correlationId) =>
        {
            Debug.Log($"GPTStressScoring callback [{correlationId}] : {response}");
            UILogger.Instance.LogStress(response);
            StressScore parsedStressScore = StressScoreParser.parse(response);
            Debug.Log($"callback parsed stress score={parsedStressScore.stressScore}, reason={parsedStressScore.reason}");
            orchestrator.StressScored(correlationId, parsedStressScore);
        };

        Action<string, int> errorCallback = (errorDescription, correlationId) =>
        {
            Debug.Log($"GPTStressScoring error callback [{correlationId}] : {errorDescription}");
            UILogger.Instance.LogStress($"Error: {errorDescription}");
            //orchestrator.StressScored(correlationId, parsedStressScore);
        };

        apiAccess.SendPrompt(promptObject, correlationId, callback, errorCallback);
    }

}
