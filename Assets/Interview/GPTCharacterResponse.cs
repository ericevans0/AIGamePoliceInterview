using PoliceInterview.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PoliceInterview.GPT
{
    public class GPTCharacterResponse : MonoBehaviour
    {
        [SerializeField] private GPTChatAPIAccess apiAccess;
        [SerializeField] private LMSelection lmSelection;
        [SerializeField] private EditablePromptTemplate characterDescriptionPromptTemplate;
        [SerializeField] private EditablePromptTemplate atEasePromptTemplate;
        [SerializeField] private EditablePromptTemplate stressedPromptTemplate;
        [SerializeField] private EditablePromptTemplate crackedPromptTemplate;


        public void IntervieweeRespond(DetectiveStatementProposal proposal, StressMode stress, List<Message> conversationSoFar, InterviewOrchestrator orchestrator)
        {
            Debug.Log($"IntervieweeRespond({proposal.Text}, {stress})");

            string stressDetermined = stressDeterminedPromptExtension(stress);
            UILogger.Instance.LogCharacter($"Prompt extension for {stress}: {stressDetermined}");

            string systemPromptTemplate = characterDescriptionPromptTemplate.PromptTemplate + stressDetermined;

            MessageGenerator tellTheLLMWhatCharacterItIsPlaying = MessageGenerator.SystemTemplate(systemPromptTemplate);

            List<Message> messages = new();
            messages.Add(tellTheLLMWhatCharacterItIsPlaying.Generate());
            messages.AddRange(conversationSoFar);

            ChatPrompt promptObject = GPTChatAPIAccess.CreateRequest(messages, lmSelection.Temperature, lmSelection.TargetLM);

            Action<string, int> callback = (response, correlationId) =>
            {
                Debug.Log($"GPTCharacterResponse callback [{correlationId}] : {response}");
                orchestrator.IntervieweeResponded(correlationId, response);
            };

            Action<string, int> errorCallback = (errorDescription, correlationId) =>
            {
                Debug.Log($"GPTCharacterResponse errorCallback [{correlationId}] : {errorDescription}");
                // orchestrator.IntervieweeResponded(correlationId, errorDescription);
            };

            apiAccess.SendPrompt(promptObject, proposal.Id, callback, errorCallback);
        }

        private string stressDeterminedPromptExtension(StressMode stressMode)
        {
            switch (stressMode)
            {
                case StressMode.AtEase:
                    return atEasePromptTemplate.PromptTemplate;
                case StressMode.Stressed:
                    return stressedPromptTemplate.PromptTemplate;
                case StressMode.Cracked:
                    return crackedPromptTemplate.PromptTemplate;
                default: return "Error: Not a valid stressMode";
            }
        }

    }
}