using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TemplatePlacer : MonoBehaviour
{
    private List<TemplateGhost> m_ghosts = new List<TemplateGhost>();

    [SerializeField]
    private GameObject m_ghostPrefab;

    private EventSystem eventSystem;

    [SerializeField]
    private Material m_ghostMaterial;

    public void Load(TemplateSO template)
    {
        Clear();
        foreach (var point in template.points)
        {
            m_ghosts.Add(Instantiate(m_ghostPrefab,
                                   transform.TransformPoint((Vector3)(Vector2)point),
                                   Quaternion.identity,
                                   transform)
                        .GetComponent<TemplateGhost>());
        }
        gameObject.SetActive(true);
    }

    public void Clear()
    {
        foreach (var ghost in m_ghosts)
        {
            Destroy(ghost.gameObject);
        }
        m_ghosts.Clear();
        gameObject.SetActive(false);
    }

    void Start()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    void Update()
    {
        Vector3 mousePoint = UIManager.GetMousePositionWorldSpace();
        transform.localPosition = new((int)mousePoint.x, (int)mousePoint.y);

        if (Input.GetMouseButtonDown((int)MouseButton.Left) && m_ghosts.Count > 0)
        {
            if (eventSystem.IsPointerOverGameObject())
            {
                return;
            }

            bool isInBounds = true;
            foreach (var ghost in m_ghosts)
            {
                if (!ghost.isInBounds)
                {
                    isInBounds = false;
                    break;
                }
            }

            if (isInBounds)
            {
                foreach (var ghost in m_ghosts)
                {
                    ghost.Place(FieldManager.GetInstance().GetActivePlayer());
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.Rotate(new Vector3(0, 0, 90));
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            transform.Rotate(new Vector3(0, 0, -90));
        }
    }
}
