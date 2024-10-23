using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

class TemplateSelector : MonoBehaviour
{
  private Vector3 m_start;
  private Vector3 m_end;
  private bool m_active = false;

  [SerializeField]
  private SpriteRenderer m_selectionDrawer;

  [SerializeField]
  private TemplateMenu menu;

  void createTemplate()
  {
    List<Vector2Int> offsets = new List<Vector2Int>();
    Vector2Int boundsX = new((int)Mathf.Min(m_start.x, m_end.x), (int)Mathf.Max(m_start.x, m_end.x));
    Vector2Int boundsY = new((int)Mathf.Min(m_start.y, m_end.y), (int)Mathf.Max(m_start.y, m_end.y));

    for (int x = boundsX.x; x <= boundsX.y; ++x)
    {
      for (int y = boundsY.x; y <= boundsY.y; ++y)
      {
        if (FieldManager.GetInstance().IsEmpty(new Vector2Int(x, y))) { continue; }

        offsets.Add(new Vector2Int(x - (boundsX.y + boundsX.x) / 2, y - (boundsY.y + boundsY.x) / 2));
      }
    }

    menu.AddTemplate(offsets, null);
  }

  void updateSelection(bool isSelected)
  {
    if (!isSelected)
    {
      m_selectionDrawer.gameObject.SetActive(false);
      return;
    }

    m_selectionDrawer.gameObject.SetActive(true);
    m_end = UIManager.GetMousePositionWorldSpace();

    m_selectionDrawer.size = m_end - m_start;
  }

  void OnEnable()
  {
    UIManager.GetInstance().SetUI(gameObject);
    updateSelection(false);
  }

  void Update()
  {
    if (Input.GetMouseButtonDown((int)MouseButton.Left))
    {
      m_start = UIManager.GetMousePositionWorldSpace();
      m_selectionDrawer.transform.position = m_start;
      m_active = true;
    }
    if (Input.GetMouseButtonUp((int)MouseButton.Left) && m_active)
    {
      m_active = false;
      createTemplate();
      updateSelection(false);
    }
    else if (m_active)
    {
      updateSelection(true);
    }
  }
}