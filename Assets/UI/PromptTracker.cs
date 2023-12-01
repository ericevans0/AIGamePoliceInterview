using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PromptTracker : MonoBehaviour
{
    [SerializeField] private Color initialColor = Color.white;
    [SerializeField] private Color successColor = Color.green;
    [SerializeField] private Color failureColor = Color.red;

    [SerializeField] private Image guardrailsSubmitted;
    [SerializeField] private Image guardrailsAnswered;
    [SerializeField] private Image guardrailsAllowed;
    [SerializeField] private TMP_Text guardrailsDetailText;

    [SerializeField] private Image stressSubmitted;
    [SerializeField] private Image stressAnswered;
    [SerializeField] private TMP_Text stressDetailText;

    [SerializeField] private Image characterPromptSubmitted;
    [SerializeField] private Image characterPromptAnswered;
    [SerializeField] private TMP_Text characterPromptDetailText;

    [SerializeField] private Image goalPromptSubmitted;
    [SerializeField] private Image goalPromptAnswered;
    [SerializeField] private Image goalPromptReached;
    [SerializeField] private TMP_Text goalPromptDetailText;


    public void GuardrailsSubmitted()
    {
        showResult(guardrailsSubmitted, true);
    }

    public void GuardrailsProcessing(bool successfulProcessing, string detail)
    {
        showResult(guardrailsAnswered, successfulProcessing);
        addDetailString(guardrailsDetailText, detail);
    }

    public void GuardrailsAnswered(bool allowed, string detail)
    {
        showResult(guardrailsAllowed, allowed);
        addDetailString(goalPromptDetailText, detail);
    }
    public void GuardrailsPromptLog(string text)
    {
        addDetailString(guardrailsDetailText, text);
    }

    public void StressSubmitted()
    {
        showResult(stressSubmitted, true);
    }

    public void StressAnswered(bool successfulProcessing, int rating)
    {
        showResult(stressAnswered, successfulProcessing);
    }

    public void StressPromptLog(string text)
    {
        addDetailString(stressDetailText, text);
    }


    public void CharacterPromptSubmitted()
    {
        showResult(characterPromptSubmitted, true);
    }

    public void CharacterPromptAnswered(bool successfulProcessing, string dialog, string detail)
    {
        showResult(characterPromptAnswered, successfulProcessing);
        addDetailString(characterPromptDetailText, detail);
    }

    public void CharacterPromptLog(string text)
    {
        addDetailString(characterPromptDetailText, text);
    }

    public void GoalPromptSubmitted()
    {
        showResult(goalPromptSubmitted, true);
    }

    public void GoalPromptAnswered(bool successfulProcessing, bool reached)
    {
        showResult(goalPromptAnswered, successfulProcessing);
        showResult(goalPromptReached, reached);
    }

    public void GoalPromptLog(string text)
    {
        addDetailString(goalPromptDetailText, text);
    }


    private void showResult(Image anIndicator, bool result)
    {
        if (anIndicator != null)
        {
            anIndicator.color = result ? successColor : failureColor;
        }
    }

    private void addDetailString(TMP_Text textField, string detail)
    {
       if (textField == null || detail == null) return;
        if (textField.text.Length > 0)
            textField.text += "\n";
        textField.text += detail;
    }

    public void ResetForNewProposedStatement()
    {
        guardrailsSubmitted.color = initialColor;
        guardrailsAnswered.color = initialColor;
        guardrailsAllowed.color = initialColor;
        if (guardrailsDetailText != null) { guardrailsDetailText.text = ""; }

        stressSubmitted.color = initialColor;
        stressAnswered.color = initialColor;
        if (stressDetailText != null) { stressDetailText.text = ""; }

        characterPromptSubmitted.color = initialColor;
        characterPromptAnswered.color = initialColor;
        if (characterPromptDetailText != null) { characterPromptDetailText.text = ""; }

        goalPromptSubmitted.color = initialColor;
        goalPromptAnswered.color = initialColor;
        goalPromptReached.color = initialColor;
        if (goalPromptDetailText != null) { goalPromptDetailText.text = ""; }
    }

}
