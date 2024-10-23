using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateSelectorEnabler : MonoBehaviour
{
    private GameObject templateSelector;

    void Start()
    {
        templateSelector = transform.Find("TemplateSelector").gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !UIManager.GetInstance().isInputCaptured)
        {
            templateSelector.SetActive(true);
        }
    }
}
