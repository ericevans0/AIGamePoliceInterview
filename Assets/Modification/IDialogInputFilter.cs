using System.Collections.Generic;

public interface IDialogInputFilter
{
    public List<Message> Filter(List<Message> messages);
}

public class All : IDialogInputFilter
{
    List<Message> IDialogInputFilter.Filter(List<Message> messages)
    {
        return messages;
    }
}

public class SystemMessagesOnly : IDialogInputFilter
{
    List<Message> IDialogInputFilter.Filter(List<Message> messages)
    {
        List<Message> result = new List<Message>();

        foreach (Message message in messages)
        {
            if (message.role == "system")
                result.Add(message);
        }

        return result;
    }
}
