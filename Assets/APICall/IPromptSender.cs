public interface IPromptSender
{
    public void PromptResponseReceived(string response, int correlationId);

    public void PromptResponseError(string error, int correlationId);
}
