using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class assembles the full chat prompt. It doesn't do Chat GPT specific stuff.
 * It can incorporate multiple messages deterrmined in different ways:
 * - Chat history, potentially filtered by a rule
 * - System prompts. Potentially more than one message, at different points in the sequence and determined by a rule
 */


public class PromptGenerator : MonoBehaviour
{
    public List<Message> GenerateMessages () => new List<Message> ();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
