using System;
using System.Collections.Generic;
using UnityEngine;

namespace PoliceInterview.GPT
{
    public class GPTGuardrails : MonoBehaviour
    {
        [SerializeField] private GPTChatAPIAccess apiAccess;
        [SerializeField] private LMSelection lmSelection;
        [SerializeField] private EditablePromptTemplate promptTemplate;
        private string currentPromptTemplate => promptTemplate.PromptTemplate;


        public void CheckGuardrails(string detectiveStatement, int correlationId, InterviewOrchestrator orchestrator)
        {
            Debug.Log($"CheckGuardrails (correlationId={correlationId},  '{detectiveStatement}'");

            string systemPrompt = currentPromptTemplate;

            MessageGenerator tellGPTWhoItIs = MessageGenerator.SystemTemplate(currentPromptTemplate);

            var variables = new Dictionary<string, string>
            {
                { "{detectiveStatement}", detectiveStatement }
            };

            MessageGenerator detectiveStatementMessageGen =
                MessageGenerator.UserTemplate("The detective says: {detectiveStatement}");

            List<Message> messages = new List<Message>()
            {
                tellGPTWhoItIs.Generate(),
                detectiveStatementMessageGen.Generate(variables)
            };

            ChatPrompt promptObject = GPTChatAPIAccess.CreateRequest(messages, lmSelection.Temperature, lmSelection.TargetLM);

            Action<string, int> errorCallback = (errorDescription, correlationId) =>
            {
                Debug.LogError($"GPTGuardrails errorCallback [{correlationId}] : {errorDescription}");
                orchestrator.GuardrailsError(correlationId, errorDescription);
            };
            

            Action<string, int> callback = (response, correlationId) =>
                {
                    Debug.Log($"GPTGuardrails callback [{correlationId}] : {response}");
                    UILogger.Instance.LogGuardrails(response);
                    try
                    {
                        GuardrailCheck parsedResponse = GuardrailCheckParser.parse(response);
                        orchestrator.GuardrailsChecked(correlationId, parsedResponse);
                    }
                    catch (Exception e)
                    {
                        Debug.Log($"Error parsing Guardrail check: {e.Message}");
                        errorCallback.Invoke($"Error parsing Guardrail check: { e.Message}", correlationId);
                    }
                };

            apiAccess.SendPrompt(promptObject, correlationId, callback, errorCallback);
        }

    }
}