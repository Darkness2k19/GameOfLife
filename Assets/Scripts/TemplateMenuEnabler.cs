using UnityEngine;

public class TemplateMenuEnabler : MonoBehaviour
{
    private GameObject templateMenu;

    void Start()
    {
        templateMenu = transform.Find("TemplateMenu").gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !UIManager.GetInstance().isInputCaptured)
        {
            templateMenu.SetActive(true);
        }
    }
}
