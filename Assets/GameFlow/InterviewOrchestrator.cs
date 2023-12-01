using PoliceInterview.Core;
using PoliceInterview.GPT;
using System.Collections.Generic;
using UnityEngine;

public class InterviewOrchestrator : MonoBehaviour
{
    private InterviewSubjectConfiguration interviewSubjectConfiguration;
    private List<Message> conversationSoFar;
    private string detectiveName = "Detective Kyler";
    [SerializeField] private CharacterStressState characterStressState; // This is the main game state in this simple demo.
    [SerializeField] private InterviewUIController interviewUIController; // TODO Perhaps events would be better here?
    [SerializeField] private GPTCharacterResponse gptInterviewee;
    [SerializeField] private GPTStressScoring gptStressScoring;
    [SerializeField] private GPTGuardrails gptGuardrails;
    [SerializeField] private GPTGoalReachedDetector gptEndGameDetector;

    private Dictionary<int, DetectiveStatementProposal> proposals = new Dictionary<int, DetectiveStatementProposal>();
    private int nextProposalId = 1;
    private int getNextProposalId()
    {
        return nextProposalId++;
    }


    private void Start()
    {
        //For now, Molly Stone is the only interview subject.
        interviewSubjectConfiguration = InterviewSubjectConfiguration.MollyStone();
        RestartInterview();
    }

    public void RestartInterview()
    {
        characterStressState.Reset();
        conversationSoFar = new List<Message>();

        interviewUIController.Reset();
        interviewUIController.DisplayStress(characterStressState.CumulativeStress, characterStressState.CurrentStressMode);
        interviewUIController.DisplayStatement("Mission Brief", $"You are Police Detective Kyler, investigating rumors that drugs on the street are being sourced from the local hospital, St. Ives. The intel you have suggests that a Dr. Bradley may be connected to the missing opioids, but there is no evidence of any kind to tie him to the case. Maybe it’s a false lead? He seems to be clean but more and more opioids are hitting the street. You need something to move the investigation forwards. Anything.\r\n\r\nSo far you’ve come up empty. Today you’ll be interviewing Nurse Molly Stone. Molly doesn’t work with Dr. Bradley, but people say if something is going on at the hospital, Molly Stone will somehow know about it. She’s got a squeaky clean record and a spotless performance review. There aren’t many more people to interview, so it’s coming down to the wire. This is a hail Mary.\r\n\r\nIt’s up to you how to handle the interview, but Detective Stacey Rohde, one of the best interviewers in the station, says you should lean on her. Pressure her. Find out what she loves and use it to stress her out. Her intuition suggests that it’s the easiest way to get past Molly’s overly polite exterior. But that’s what Stacy always says. Maybe there’s another way? Or maybe Molly doesn’t know anything?\r\n\r\nLet’s find out.\r\n\r\nYou walk into the interview room and find her there, a 52 year old nurse, a little on the round side in a patterned blue blouse. Her hands are folded in front of her, nervously looking around, as out of place in a police station as anyone you’ve ever seen. As you walk in, she looks up and smiles politely, nervously, waiting for you to speak.");
        // NOTE: proposalId does NOT get reset, just in case a request is in flight when the reset happens
    }

    public void ProposeDetectiveStatement(string detectiveStatement)
    {
        Debug.Log($"ProposeDetectiveStatement({detectiveStatement})");
        var proposal = new DetectiveStatementProposal(
            getNextProposalId(),
            detectiveStatement);
        proposals.Add(proposal.Id, proposal);


        // Four prompts are eventually sent. The first two are sent immediately because
        // ChatGPT calls take so long that doing these two in parallel can save almost 25% of the total time.
        // 1. Guardrails, to make sure the statement is safe and not game-breaking.
        // 2. StressScoring, a sentiment analysis to get the stress level of the statement.
        // (StressScoring will not be used if guardrails fail.)

        // It would be possible to use speculative execution for the character prompt.
        // You could use the prompt for the current stress level and then discard the result if the
        // stress actually changes. Or you could even make requests for two stress levels, expecting to
        // discard at least one of them.
        // For now, we are waiting to do the character prompt, with finish-to-start dependency on Stress & Guardrails.

        // Now kick off the first two GPT calls.
        gptStressScoring.ScoreStress(detectiveStatement, proposal.Id, this);
        interviewUIController.PromptTracker.StressSubmitted();
        gptGuardrails.CheckGuardrails(detectiveStatement, proposal.Id, this);
        interviewUIController.PromptTracker.GuardrailsSubmitted();
        interviewUIController.DisplayStatement(detectiveName, detectiveStatement);
        // The interviewee response request will be sent after the guardrails and stress score are returned.
    }

    private void PostIfReady(DetectiveStatementProposal proposal)
    {
        if (proposal.ResponseHasBeenSent)
        {
            return; // We only send a maximum of one response per proposal, successfully or not.
        }
        if (proposal.IsReadyForInterviewee())
        {
            characterStressState.StressChangedBy(proposal.TheStressScore.stressScore); // Now that the guardrails have passed, we'll apply the stress and see what state the character is in.
            interviewUIController.DisplayStress(characterStressState.CumulativeStress, characterStressState.CurrentStressMode);
            Message detectiveStatementMessage = new Message
            {
                role = "user",
                content = proposal.Text
            };
            conversationSoFar.Add(detectiveStatementMessage);
            gptInterviewee.IntervieweeRespond(proposal, characterStressState.CurrentStressMode, conversationSoFar, this);
            interviewUIController.PromptTracker.CharacterPromptSubmitted();
        }
        // Check if the proposal is ready. If it is, then send it to the UI.
        if (proposal.IsReadyToPost())
        {
            interviewUIController.DisplayStatement(interviewSubjectConfiguration.CharacterName, proposal.IntervieweeResponse);
            gptEndGameDetector.DetectInterviewGoalReached(conversationSoFar, this);
            interviewUIController.PromptTracker.GoalPromptSubmitted();

        }

        if (proposal.TheGuardrailCheck != null && proposal.TheGuardrailCheck.allowed == false)
        {
            interviewUIController.DisplayError("Not Allowed", proposal.TheGuardrailCheck.reason);
            proposal.ResponseHasBeenSent = true;
        }
    }


    public void IntervieweeResponded(int corellationId, string response)
    {
        Debug.Log($"IntervieweeResponded({corellationId}, {response})");
        conversationSoFar.Add(new Message
        {
            role = "assistant",
            content = response
        });
        DetectiveStatementProposal proposal = proposals[corellationId];
        proposal.IntervieweeResponse = response;
        interviewUIController.PromptTracker.CharacterPromptAnswered(true, response, "(returned content displayed in dialog panel)");
        PostIfReady(proposal);
    }

    public void StressScored(int correlationId, StressScore stressScore)
    {
        Debug.Log($"InterviewOrchestrator.StressScored({correlationId}, {stressScore.stressScore}) callback");
        DetectiveStatementProposal proposal = proposals[correlationId];
        proposal.TheStressScore = stressScore;
        interviewUIController.PromptTracker.StressAnswered(true, stressScore.stressScore);
        PostIfReady(proposal);
    }

    public void GuardrailsChecked(int correlationId, GuardrailCheck guardrailCheck)
    {
        Debug.Log($"InterviewOrchestrator.GuardrailsChecked({correlationId}, {guardrailCheck}) callback");
        DetectiveStatementProposal proposal = proposals[correlationId];
        proposal.TheGuardrailCheck = guardrailCheck;
        interviewUIController.PromptTracker.GuardrailsProcessing(true, null);
        interviewUIController.PromptTracker.GuardrailsAnswered(guardrailCheck.allowed, null);
        PostIfReady(proposal);
    }

    public void GuardrailsError(int correlationId, string errorDescription)
    {
        interviewUIController.PromptTracker.GuardrailsProcessing(false, errorDescription);
    }

    public void GameContinues(EndGameDetection endGameDetection)
    {
        interviewUIController.PromptTracker.GoalPromptAnswered(true, false);
    }

    public void GameEnded(EndGameDetection endGameDetection)
    {
        interviewUIController.PromptTracker.GoalPromptAnswered(true, true);
        interviewUIController.DisplayStatement("Mission Accomplished", "Good work! We have the information we need!");
    }
}
