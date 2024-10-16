using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemplateItem : MonoBehaviour
{
    public Text templateName;
    public Button removeButton;
    public TemplateSO template;
    public TemplateMenu menu;

    public void Delete()
    {
        menu.DeleteTemplate(this);
    }
}
