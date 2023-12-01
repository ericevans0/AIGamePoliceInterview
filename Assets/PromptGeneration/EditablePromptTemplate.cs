using UnityEngine;

public class EditablePromptTemplate : MonoBehaviour, IEditableBasicPrompt
{
    [SerializeField]
    [TextArea(3, 50)]
    private string defaultPromptTemplate = "";
    private string promptTemplateOverride;

    private string currentPromptTemplate => (promptTemplateOverride == null) ? defaultPromptTemplate : promptTemplateOverride;

    public string PromptTemplate => currentPromptTemplate;

    public void Apply(string newPromptTemplate)
    {
        promptTemplateOverride = newPromptTemplate;
    }

    public void RestoreDefault()
    {
        promptTemplateOverride = null;
    }

}
