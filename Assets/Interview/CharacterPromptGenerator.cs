using PoliceInterview.Core;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterPromptGenerator : MonoBehaviour
{
    /**

    [SerializeField] private EditablePromptTemplate characterDescriptionPromptTemplate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private List<MessageGenerator> stressDeterminedPromptExtension(StressMode stressMode)
    {
        var result = new List<MessageGenerator>();
        if (stressMode == StressMode.AtEase) result.Add(MessageGenerator.SystemTemplate(atEasePromptTemplate.PromptTemplate));
        if (stressMode == StressMode.Stressed) result.Add(MessageGenerator.SystemTemplate(stressedPromptTemplate.PromptTemplate));
        if (stressMode == StressMode.Cracked) result.Add(MessageGenerator.SystemTemplate(crackedPromptTemplate.PromptTemplate));
        return result;
    }


    public List<Message> GenerateMessages(List<Message> chatHistory)
    {
        MessageGenerator tellTheLLMWhatCharacterItIsPlaying = MessageGenerator.SystemTemplate(characterDescriptionPromptTemplate.PromptTemplate);
        string systemPrompt = characterDescriptionPromptTemplate.PromptTemplate;

        List<Message> messages = new();
        messages.Add(tellTheLLMWhatCharacterItIsPlaying.Generate());
        messages.AddRange(chatHistory);
        foreach (MessageGenerator messageGenerator in stressDeterminedPromptExtension(stress))
        {
            messages.Add(messageGenerator.Generate());
        }
    }
    */

}
