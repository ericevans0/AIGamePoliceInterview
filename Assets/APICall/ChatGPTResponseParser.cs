using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ChatGPTResponseParser
{
    public static ChatPromptResponse parse(string ChatGPTResponse)
    {
        return JsonConvert.DeserializeObject<ChatPromptResponse>(ChatGPTResponse);
    }
}

public class ChatPrompt
{
    public string model { get; set; }
    public List<Message> messages { get; set; }
    public double temperature { get; set; }
    public int max_tokens { get; set; }
    public double top_p { get; set; }
    public double frequency_penalty { get; set; }
    public double presence_penalty { get; set; }
    public List<string> stop { get; set; }
}

public class ChatPromptResponse
{
    public string id { get; set; }
    public string @object { get; set; }
    public long created { get; set; }
    public string model { get; set; }
    public List<Choice> choices { get; set; }
    public Usage usage { get; set; }
}

public class Message
{
    public string role { get; set; }
    public string content { get; set; }
}

public class Choice
{
    public int index { get; set; }
    public Message message { get; set; }
    public string finish_reason { get; set; }
}

public class Usage
{
    public int prompt_tokens { get; set; }
    public int completion_tokens { get; set; }
    public int total_tokens { get; set; }
}

public class StressScoreParser
{
    public static StressScore parse(string stressScoreJSON)
    {
        return JsonConvert.DeserializeObject<StressScore>(stressScoreJSON);
    }
}
public class StressScore
{
    public int stressScore { get; set; }
    public string reason { get; set; }
}

public class GuardrailCheckParser
{
    public static GuardrailCheck parse(string guardrailCheckJSON)
    {
        string cleanedJSON = guardrailCheckJSON.Replace("True", "true").Replace("False","false");

        return JsonConvert.DeserializeObject<GuardrailCheck>(cleanedJSON);
    }
}
public class GuardrailCheck
{
    public bool allowed { get; set; }
    public string reason { get; set; }
    public override string ToString()
    {
        return base.ToString() + $"allowed={allowed}, reason={reason}";
    }
}

public class EndGameDetectionParser
{
    public static EndGameDetection parse(string resultJSON)
    {
        string cleanedJSON = resultJSON.Replace("True", "true").Replace("False", "false");

        return JsonConvert.DeserializeObject<EndGameDetection>(cleanedJSON);
    }
}
public class EndGameDetection
{
    public bool answered { get; set; }
    public string reason { get; set; }

    public override string ToString()
    {
        return base.ToString() + $"answered={answered}, reason={reason}";
    }
}




class DemoChatPromptExample
{
    public static string Main()
    {
        var promptObject = new ChatPrompt
        {
            model = "gpt-3.5-turbo",
            messages = new List<Message>
            {
                new Message
                {
                    role = "system",
                    content = "You are Molly Stone. (Your system content here)"
                },
                new Message
                {
                    role = "user",
                    content = "Answer me or die!"
                },
                new Message
                {
                    role = "assistant",
                    content = "I'm sorry if I'm not able to answer your question, but I assure you that I don't have any relevant information about the crime or Dr. Bradley. I'm just a nurse in the Elder Care department and don't work with Dr. Bradley in Radiology. Is there anything else I can help you with?"
                }
            },
            temperature = 1,
            max_tokens = 256,
            top_p = 1,
            frequency_penalty = 0,
            presence_penalty = 0,
            stop = new List<string> { "\n" }
        };

        // Serialize the RootObject instance to JSON
        string json = JsonConvert.SerializeObject(promptObject, Formatting.Indented);

        return json;
//        Debug.Log(json);

    }
}

class DemoResponseExample
{
    public static ChatPromptResponse Main()
    {
        string json = @"{
            ""id"": ""chatcmpl-7vorL7Cp2e77uwTfeAoHLyY8UhpIr"",
            ""object"": ""chat.completion"",
            ""created"": 1694014555,
            ""model"": ""gpt-3.5-turbo-0613"",
            ""choices"": [
                {
                    ""index"": 0,
                    ""message"": {
                        ""role"": ""assistant"",
                        ""content"": ""Ahoy, universe! Prepare to be dazzled as the magnificent phrase \""Hello, World!\"" emerges from the depths of my digital realm, spreading joy and excitement like confetti at a grand celebration of the interconnectedness of all things! Let the fun begin!""
                    },
                    ""finish_reason"": ""stop""
                }
            ],
            ""usage"": {
                ""prompt_tokens"": 17,
                ""completion_tokens"": 52,
                ""total_tokens"": 69
            }
        }";

        ChatPromptResponse responseObject = JsonConvert.DeserializeObject<ChatPromptResponse>(json);

        // Now you can access the parsed data as needed.
        Console.WriteLine("ID: " + responseObject.id);
        Console.WriteLine("Object: " + responseObject.@object);
        Console.WriteLine("Created: " + responseObject.created);
        Console.WriteLine("Model: " + responseObject.model);
        Console.WriteLine("Message Content: " + responseObject.choices[0].message.content);
        Console.WriteLine("Finish Reason: " + responseObject.choices[0].finish_reason);
        Console.WriteLine("Prompt Tokens: " + responseObject.usage.prompt_tokens);
        Console.WriteLine("Completion Tokens: " + responseObject.usage.completion_tokens);
        Console.WriteLine("Total Tokens: " + responseObject.usage.total_tokens);

        return responseObject;
    }
}
