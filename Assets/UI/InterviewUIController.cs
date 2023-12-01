using PoliceInterview.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InterviewUIController : MonoBehaviour
{
    [SerializeField] private TMP_InputField openAiApiKeyInputField;
    [SerializeField] private TMP_InputField promptInputField;
    [SerializeField] private TMP_Text responseDisplay;
    [SerializeField] private TMP_Text stressDisplay;
    [SerializeField] private InterviewOrchestrator orchestrator;
    [SerializeField] private PromptTracker promptTracker;
    [SerializeField] private ScrollRect scrollRect;

    public PromptTracker PromptTracker { get { return promptTracker; }}

    public void ProposeDetectiveStatement()
    {
        Debug.Log("ProposeDetectiveStatement()");
        PromptTracker.ResetForNewProposedStatement();
        string inputString = promptInputField.text.Trim();
        if (inputString.Length > 0)
        {
            orchestrator.ProposeDetectiveStatement(inputString);
            promptInputField.text = "";
        }
    }

    public void DisplayStatement(string speakerName, string statement)
    {
        responseDisplay.text += $"\n\n<b>{speakerName}:</b> {statement}";
        StartCoroutine(ScrollToBottom());
    }

    public IEnumerator ScrollToBottom()
    {
        // Wait for the end of the frame so that UI elements can update their layout
        yield return new WaitForEndOfFrame();

        // After UI update, set the vertical position to 0 to scroll to the bottom
        scrollRect.verticalNormalizedPosition = 0f;
    }
    public void DisplayError(string errorName, string errorDescription)
    {
        Debug.Log("ERROR" + errorDescription);
        responseDisplay.text += $"\n\n<color=red><b>{errorName}:</b> {errorDescription}</color>";
        StartCoroutine(ScrollToBottom());
    }

    public void DisplayStress(int level, StressMode mode)
    {
        stressDisplay.text = $"{mode} (Stress:{level})";
    }

    public void Reset()
    {
        responseDisplay.text = null ;
        promptInputField.text = null ;
        PromptTracker.ResetForNewProposedStatement();
    }
}
