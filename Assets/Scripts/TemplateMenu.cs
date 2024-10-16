using System.Collections.Generic;
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

    public List<TemplateSO> addedTemplates = new List<TemplateSO>();
    private HashSet<TemplateSO> m_templates = new HashSet<TemplateSO>();
    private HashSet<GameObject> m_items = new HashSet<GameObject>();

    public void ActivateTemplate(TemplateItem item)
    {
        m_placer.Load(item.template);
        gameObject.SetActive(false);
    }

    public void DeleteTemplate(TemplateItem item)
    {
        m_templates.Remove(item.template);
        m_items.Remove(item.gameObject);
        Destroy(item.gameObject);
        updateContainerSize();
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
        foreach (var template in addedTemplates)
        {
            m_templates.Add(template);
        }
    }

    void OnEnable()
    {
        foreach (var template in m_templates)
        {
            var itemPrefab = Instantiate(m_templateItemPrefab, m_container.transform);
            var templateItem = itemPrefab.GetComponent<TemplateItem>();
            templateItem.template = template;
            templateItem.menu = this;
            templateItem.templateName.text = template.name;

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
