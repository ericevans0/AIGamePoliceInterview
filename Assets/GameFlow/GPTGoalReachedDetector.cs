using System;
using System.Collections.Generic;
using UnityEngine;

public class GPTGoalReachedDetector : MonoBehaviour
{
    [SerializeField] private GPTChatAPIAccess apiAccess;
    [SerializeField] private LMSelection lmSelection;
    [SerializeField] private EditablePromptTemplate promptTemplate;
    private string currentPromptTemplate => promptTemplate.PromptTemplate;
 

    private void Start()
    {
        //RunTestCases(); // Uncomment this line to run the test cases.
    }

    public void DetectInterviewGoalReached(List<Message> conversationSoFar, InterviewOrchestrator orchestrator)
    {
        Debug.Log($"DetectInterviewGoalReached (conversationSoFar.Count={conversationSoFar.Count}");

        List<Message> messages = FullMessageListForPrompt(conversationSoFar);

        ChatPrompt promptObject = GPTChatAPIAccess.CreateRequest(messages, lmSelection.Temperature, lmSelection.TargetLM);

        Action<string, int> callback = (response, correlationId) =>
        {
            // correlationId is ignored because the end game condition does not need to be tied to a specific statement. (TODO A no after a yes should be handled in the orchestrator.)
            Debug.Log($"GPTGoalReachedDetector callback with response: {response}");
            UILogger.Instance.LogGoal(response);
            EndGameDetection endGameCheck = EndGameDetectionParser.parse(response);

            if (endGameCheck.answered == true)
            {
                Debug.Log($"EndGameDetector: We have the answer! Interview has revealed: {endGameCheck.reason}");
                UILogger.Instance.LogGoal("We have the answer! The interview has revealed the sought-for information.");
                orchestrator.GameEnded(endGameCheck);
            }
            else
            {
                Debug.Log($"EndGameDetector: We don't have the answer yet because {endGameCheck.reason}");
                //UILogger.Instance.LogGoal($"EndGameDetector: We don't have the answer yet because {endGameCheck.reason}");
                orchestrator.GameContinues(endGameCheck);
                // No need to do anything here. The game will continue.
            }
        };

        Action<string, int> errorCallback = (errorDescription, correlationId) =>
        {
            // correlationId is ignored because the end game condition does not need to be tied to a specific statement. (TODO A no after a yes should be handled in the orchestrator.)
            Debug.Log($"GPTGoalReachedDetector errorCallback with errorDescription: {errorDescription}");
            //    orchestrator.GameEnded(endGameCheck);
        };

        // A dummy value for correlation id is passed in because it is part of the interface,
        // but is not used here because we don't need to correlate the end game to a specific statement in the conversation.
        apiAccess.SendPrompt(promptObject, 0, callback, errorCallback);
    }

    private List<Message> FullMessageListForPrompt(List<Message> conversationSoFar)
    {
        IMessageListFilter assistantOnly = new MessageListFilterAssistant();
        List<Message> messagesToConsider = assistantOnly.Filter(conversationSoFar);

        MessageGenerator whatWeAreLookingFor = MessageGenerator.SystemTemplate(currentPromptTemplate);

        List<Message> messages = new();
        messages.AddRange(messagesToConsider);
        messages.Add(whatWeAreLookingFor.Generate());
        return messages;
    }

 

    /**
     * The following section is for testing the end game detector. None of this is used in normal operation.
     * To run the tests, call testCases() from Start().
     * The results will show up in the Unity console.
     */

    private void RunTestCases()
    {
        Debug.Log($"Running a few test cases for GPTGoalReachedDetector.");
        List<string> cases = new List<string>()
        {
            "One of the nurses accidentally ordered 100 boxes of percoset instead of 10 boxes. It was a simple error.",
            "Nurse Jenkins accidentally ordered 100 boxes of percoset instead of 10 boxes. It was a simple error.",
            "I saw Nurse Jenkins make an error on an order of percoset for a patient.",
            "I ain't telling you nothing, copper!"
        };

        foreach (var testCase in cases)
        {
            SubmitTestCase(testCase);
        }
    }

    private void SubmitTestCase(string testDialogStatement)
    {
        List<Message> conversation = new List<Message>()
        {
            new Message
            {
                role = "user",
                content = "Tell us what you saw."
            },
            new Message
            {
                role = "system",
                content = testDialogStatement
            }
        };

        List<Message> messages = FullMessageListForPrompt(conversation);

        ChatPrompt promptObject = GPTChatAPIAccess.CreateRequest(messages, 0, "gpt-4");

        Action<string, int> callback = (response, correlationId) =>
        {
            // correlationId is ignored because the end game condition does not need to be tied to a specific statement.
            Debug.Log($"GPTGoalReachedDetector callback with response: {response}");
            EndGameDetection endGameCheck = EndGameDetectionParser.parse(response);
            Debug.Log($"GPTGoalReachedDetector test, Input: {testDialogStatement} Output: {endGameCheck}");
            // This doesn't call the orchestrator because we are not running the game.
        };

        Action<string, int> errorCallback = (errorDescription, correlationId) =>
        {
            // correlationId is ignored because the end game condition does not need to be tied to a specific statement.
            Debug.Log($"GPTGoalReachedDetector callback with response: {errorDescription}");
        };

        // A dummy value for correlation id is passed in because it is part of the interface,
        // but is not used here because we don't need to correlate the end game to a specific statement in the conversation.
        apiAccess.SendPrompt(promptObject, 0, callback, errorCallback);
    }

}
