using System.Collections.Generic;

public class MessageGenerator
{
    private string role;
    private string template;

    public static MessageGenerator SystemTemplate(string promptTemplate)
    {
       return new MessageGenerator("system", promptTemplate);        
    }
    public static MessageGenerator UserTemplate(string promptTemplate)
    {
        return new MessageGenerator("user", promptTemplate);
    }

    public MessageGenerator(string role, string promptTemplate)
    {
        this.role = role;
        this.template = promptTemplate;
    }

    public Message Generate(Dictionary<string,string> variables = null) 
    {
        return new Message
        {
            role = this.role,
            content = (variables == null) ? this.template : substituteVariablesInTemplate(this.template, variables)
        };
    }

    private string substituteVariablesInTemplate(string aTemplate, Dictionary<string,string> substitutions)
    {
        string result = aTemplate;
        foreach (var key in substitutions.Keys)
        {
            var value = substitutions[key];
            result = result.Replace(key, value);
        }
        return result;
    }
}
