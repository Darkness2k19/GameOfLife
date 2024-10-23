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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            templateSelector.SetActive(true);
        }
    }
}
