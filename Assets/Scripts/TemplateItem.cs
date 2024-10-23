using UnityEngine;
using UnityEngine.UI;

public class TemplateItem : MonoBehaviour
{
    public Text templateNameText;
    public TemplateSO template;
    public TemplateMenu menu;

    [SerializeField]
    private GameObject renamingButton;

    [SerializeField]
    private GameObject renamingInputField;

    public void ActivateRenaming()
    {
        renamingInputField.GetComponent<InputField>().text = template.name;

        renamingButton.SetActive(false);
        renamingInputField.SetActive(true);

        UIManager.GetInstance().isInputCaptured = true;
    }

    public void Rename(string name)
    {
        menu.RenameTemplate(this, name);
        renamingButton.SetActive(true);
        renamingInputField.SetActive(false);

        templateNameText.text = name;

        UIManager.GetInstance().isInputCaptured = false;
    }

    public void Activate()
    {
        menu.ActivateTemplate(this);
    }

    public void Save()
    {
        menu.SaveTemplate(this);
    }

    public void Delete()
    {
        menu.DeleteTemplate(this);
    }
}
