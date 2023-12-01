using TMPro;
using UnityEngine;

public class PromptEditor : MonoBehaviour
{
    [SerializeField] private GameObject promptManagerObject;
    private IEditableBasicPrompt promptManager;
    [SerializeField] private TMP_InputField promptTemplateInputField;

    private void populateEditorFromPromptManager()
    {
        promptManager = promptManagerObject.GetComponent<IEditableBasicPrompt>();
       promptTemplateInputField.text = promptManager.PromptTemplate;
    }
    void Start()
    {
        populateEditorFromPromptManager();
    }

    public void Apply()
    {
        promptManager.Apply(promptTemplateInputField.text.Trim());
        populateEditorFromPromptManager();
    }

    public void RestoreDefault()
    { 
        promptManager.RestoreDefault();
        populateEditorFromPromptManager();
    }

    public void CancelAndAbandonChangesSinceLastApply()
    {
        populateEditorFromPromptManager();
    }
}

