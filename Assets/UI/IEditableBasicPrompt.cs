
public interface IEditableBasicPrompt
{
    string PromptTemplate { get; }

    void Apply(string newPromptTemplate);

    void RestoreDefault();
}
