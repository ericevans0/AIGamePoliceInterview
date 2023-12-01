using PoliceInterview.Core;
using UnityEngine;

public class CharacterStressState : MonoBehaviour
{
    // The only real game state is how stressed out Molly Stone is. This calculates that and interprets thresholds.

    [SerializeField] private int ThresholdForStressed = 1;
    [SerializeField] private int ThresholdForCracked = 7;

    private int cumulativeStress = 0;
    public int CumulativeStress { get { return cumulativeStress; } }

    private StressMode currentStressMode;
    public StressMode CurrentStressMode { get { return currentStressMode; } }

    public void StressChangedBy(int stressChange)
    {
        cumulativeStress += stressChange;
        currentStressMode = scoreToMode(cumulativeStress);
    }

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        cumulativeStress = 0;
        currentStressMode = scoreToMode(cumulativeStress);
    }


    private StressMode scoreToMode(int score)
    {
        if (score < ThresholdForStressed)
        {
            return StressMode.AtEase;
        }
        else if (ThresholdForStressed <= score && score < ThresholdForCracked)
        {
            return StressMode.Stressed;
        }
        else
        {
            return StressMode.Cracked;
        }
    }
}
