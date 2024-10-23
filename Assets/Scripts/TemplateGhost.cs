using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateGhost : MonoBehaviour
{
    public SpriteRenderer renderer;
    public bool isInBounds = false;

    public void Place(int holderId)
    {
        Vector3 position = transform.position;
        FieldManager.GetInstance().UpdateCell(new Vector2Int((int)position.x, (int)position.y), holderId);
    }

    void Update()
    {
        Vector3 position = transform.position;
        if (position.x < 0 ||
            position.y < 0 ||
            position.x >= FieldManager.GetInstance().GetBounds().x ||
            position.y >= FieldManager.GetInstance().GetBounds().y)
        {
            renderer.material.SetColor("_Color", Color.red);
            isInBounds = false;
        }
        else
        {
            renderer.material.SetColor("_Color", Color.green);
            isInBounds = true;
        }
    }
}
