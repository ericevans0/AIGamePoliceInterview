using System.Collections.Generic;

public interface IMessageListFilter
{
    List<Message> Filter(List<Message> list);
}

public class MessageListFilterAny : IMessageListFilter
{
    List<Message> IMessageListFilter.Filter(List<Message> list)
    {
        return list;
    }
}

public class MessageListFilterAssistant : IMessageListFilter
{
    List<Message> IMessageListFilter.Filter(List<Message> list)
    {
        var result = new List<Message>();
        foreach (var message in list) 
        { 
            if (message.role == "assistant") result.Add(message);
        }
        return result;
    }
}
