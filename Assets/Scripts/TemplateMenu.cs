using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TemplateMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject m_templateItemPrefab;
    private RectTransform m_templateItemPrefabTransform;

    [SerializeField]
    private GridLayoutGroup m_container;
    private RectTransform m_containerTransform;

    [SerializeField]
    private TemplatePlacer m_placer;

    private HashSet<TemplateSO> m_templates = new HashSet<TemplateSO>();
    private HashSet<GameObject> m_items = new HashSet<GameObject>();

    [SerializeField]
    private string templatesDirectory = "Prefabs/Templates";

    public TemplateSO AddTemplate(List<Vector2Int> points, string name)
    {
        if (name == null)
        {
            name = "name" + m_templates.Count.ToString();
        }

        var template = ScriptableObject.CreateInstance(typeof(TemplateSO).Name) as TemplateSO;
        template.name = name;
        template.points = points;
        m_templates.Add(template);

        return template;
    }

    public void ActivateTemplate(TemplateItem item)
    {
        UIManager.GetInstance().SetUI(m_placer.gameObject);
        m_placer.Load(item.template, 0);
    }

    public void SaveTemplate(TemplateItem item)
    {
        AssetDatabase.CreateAsset(item.template, string.Format("Assets/Resources/{0}/{1}.asset", templatesDirectory, item.template.name));
    }

    public void RenameTemplate(TemplateItem item, string newName)
    {
        if (newName == item.template.name)
        {
            return;
        }

        var template = AddTemplate(item.template.points, newName);
        m_templates.Remove(item.template);
        clearAsset(item);

        item.template = template;
        item.templateNameText.text = template.name;
        SaveTemplate(item);
    }

    public void DeleteTemplate(TemplateItem item)
    {
        m_templates.Remove(item.template);
        clearAsset(item);

        m_items.Remove(item.gameObject);
        Destroy(item.gameObject);
        updateContainerSize();
    }

    private void clearAsset(TemplateItem item)
    {
        AssetDatabase.DeleteAsset(string.Format("Assets/Resources/{0}/{1}.asset", templatesDirectory, item.template.name));
    }

    private void updateContainerSize()
    {
        m_containerTransform.sizeDelta = new Vector2(
            m_containerTransform.rect.width,
            20 +
            m_templateItemPrefabTransform.rect.height * ((m_templates.Count + 1) / 2) +
            m_container.spacing.y * ((m_templates.Count - 1) / 2)
        );
    }

    void Awake()
    {
        m_templateItemPrefabTransform = m_templateItemPrefab.GetComponent<RectTransform>();
        m_containerTransform = m_container.GetComponent<RectTransform>();
        var loadedTemplates = Resources.LoadAll<TemplateSO>(templatesDirectory);
        foreach (var template in loadedTemplates)
        {
            m_templates.Add(template);
        }
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        UIManager.GetInstance().SetUI(gameObject);
        foreach (var template in m_templates)
        {
            var itemPrefab = Instantiate(m_templateItemPrefab, m_container.transform);
            var templateItem = itemPrefab.GetComponent<TemplateItem>();
            templateItem.template = template;
            templateItem.menu = this;
            templateItem.templateNameText.text = template.name;

            m_items.Add(itemPrefab);
        }
        updateContainerSize();
    }

    void OnDisable()
    {
        foreach (var item in m_items)
        {
            Destroy(item);
        }
        m_items.Clear();
    }
}
