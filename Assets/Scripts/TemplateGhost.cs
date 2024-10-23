using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateGhost : MonoBehaviour
{
    public void Place(int holderId)
    {
        Vector3 position = transform.position;
        FieldManager.GetInstance().UpdateCell(new Vector2Int((int)position.x, (int)position.y), holderId);
    }
}
