using System.Collections;
using System.Collections.Generic;
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
        if (Input.GetKeyDown(KeyCode.T))
        {
            templateMenu.SetActive(true);
        }
    }
}
