using UnityEngine;

public class UILogger : MonoBehaviour
{
    [SerializeField] private PromptTracker promptTracker;

    private static UILogger instance;

    public static UILogger Instance
    { 
        get 
        { 
            return instance; 
        } 
    }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void LogGuardrails(string text)
    {
        promptTracker.GuardrailsPromptLog(text);
    }

    public void LogStress(string text)

    {
        promptTracker.StressPromptLog(text);
    }

    public void LogCharacter(string text)
    {
       promptTracker.CharacterPromptLog(text);
    }

    public void LogGoal(string text)
    {
        promptTracker.GoalPromptLog(text);
    }

}
