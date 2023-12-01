using UnityEngine;

public class LMSelection : MonoBehaviour
{
    private enum AvailableLMs
    {
        GPT3_5,
        GPT4
    }

    private string nameForLMInAPI(AvailableLMs lm)
    {
        switch (lm)
        {
            case AvailableLMs.GPT3_5:
                return "gpt-3.5-turbo";
            case AvailableLMs.GPT4:
                return "gpt-4";
            default:
                return "Error: Unknown language model";
        }
    }

    [SerializeField] 
    private AvailableLMs targetLM;

    public string TargetLM
    {
        get { return nameForLMInAPI(targetLM); }
    }

    [Range(0f, 1f)]
    [SerializeField] 
    private float temperature = 0f;

    public double Temperature { get { return temperature; } }
}
