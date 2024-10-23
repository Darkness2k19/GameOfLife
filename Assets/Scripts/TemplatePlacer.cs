using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TemplatePlacer : MonoBehaviour
{
    public List<TemplateGhost> ghosts = new List<TemplateGhost>();

    [SerializeField]
    private GameObject m_ghostPrefab;

    private int m_holderId = -1;

    private EventSystem eventSystem;

    public void Load(TemplateSO template, int holderId)
    {
        Clear();
        foreach (var point in template.points)
        {
            ghosts.Add(Instantiate(m_ghostPrefab,
                                   transform.TransformPoint((Vector3)(Vector2)point),
                                   Quaternion.identity,
                                   transform)
                        .GetComponent<TemplateGhost>());
        }
        m_holderId = holderId;
        gameObject.SetActive(true);
    }

    public void Clear()
    {
        foreach (var ghost in ghosts)
        {
            Destroy(ghost.gameObject);
        }
        ghosts.Clear();
        m_holderId = -1;
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

        if (Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            if (eventSystem.IsPointerOverGameObject())
            {
                return;
            }
            foreach (var ghost in ghosts)
            {
                ghost.Place(m_holderId);
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
