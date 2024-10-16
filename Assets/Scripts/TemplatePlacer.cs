using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplatePlacer : MonoBehaviour
{
    [SerializeField]
    private FieldManager m_field;
    public List<GameObject> ghosts = new List<GameObject>();

    [SerializeField]
    private GameObject m_ghostPrefab;

    public void Load(TemplateSO template)
    {
        foreach (var point in template.points)
        {
            ghosts.Add(Instantiate(m_ghostPrefab, (Vector3)(Vector2)point, Quaternion.identity, transform));
        }
    }

    public void Spawn()
    {
        foreach (var ghost in ghosts)
        {
            Debug.Log(ghost.transform.position);
        }
    }

    public void Clear()
    {
        foreach (var ghost in ghosts)
        {
            Destroy(ghost);
        }
        ghosts.Clear();
    }
}
