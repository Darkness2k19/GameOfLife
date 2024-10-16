using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateMenuEnabler : MonoBehaviour
{
    [SerializeField]
    private GameObject templateMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            templateMenu.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            templateMenu.SetActive(false);
        }
    }
}
