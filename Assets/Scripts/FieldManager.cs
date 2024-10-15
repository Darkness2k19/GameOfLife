using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FieldManager : MonoBehaviour
{
    [SerializeField]
    private TileBase m_tile;

    [SerializeField]
    private Vector2Int m_fieldSize;
    [SerializeField]
    private Cell[,] m_field;
    private Tilemap m_tilemap;

    [SerializeField]
    [Range(0.25f, 10f)]
    private float m_timeSpeed = 1f;

    private void generate()
    {
        m_field = new Cell[m_fieldSize.x, m_fieldSize.y];

        int cells = 0;
        for (int x = 0; x < m_fieldSize.x; ++x)
        {
            for (int y = 0; y < m_fieldSize.y; ++y)
            {
                m_tilemap.SetTile(new Vector3Int(x, y), m_tile);
                ref Cell cell = ref m_field[x, y];
                var randomValue = Random.Range(0, 2);
                cell = new Cell(m_tilemap, new Vector2Int(x, y), randomValue > 0 ? -1 : 0);
                if (!cell.IsEmpty())
                {
                    ++cells;
                }
            }
        }
        Debug.LogFormat("Cells on start: {0}", cells);
    }

    private void updateCell(Vector2Int position)
    {
        uint neighbours = 0;
        Vector2Int xBounds = new(Mathf.Max(position.x - 1, 0), Mathf.Min(position.x + 1, m_fieldSize.x - 1));
        Vector2Int yBounds = new(Mathf.Max(position.y - 1, 0), Mathf.Min(position.y + 1, m_fieldSize.y - 1));
        for (int x = xBounds.x; x <= xBounds.y; ++x)
        {
            for (int y = yBounds.x; y <= yBounds.y; ++y)
            {
                if (x == position.x && y == position.y)
                {
                    continue;
                }
                if (!m_field[x, y].IsEmpty())
                {
                    ++neighbours;
                }
            }
        }
        if (m_field[position.x, position.y].IsEmpty())
        {
            if (neighbours == 3)
            {
                m_field[position.x, position.y].holderId = 0;
            }
        }
        else
        {
            if (neighbours != 2 && neighbours != 3)
            {
                m_field[position.x, position.y].holderId = -1;
            }
        }
    }

    void Start()
    {
        m_tilemap = GetComponent<Tilemap>();
        generate();

        StartCoroutine(updateCells());
    }

    private IEnumerator updateCells()
    {
        uint cells = 1;
        while (cells > 0)
        {
            yield return new WaitForSeconds(0.5f / m_timeSpeed);
            for (int x = 0; x < m_fieldSize.x; ++x)
            {
                for (int y = 0; y < m_fieldSize.y; ++y)
                {
                    updateCell(new Vector2Int(x, y));
                }
            }

            cells = 0;
            foreach (Cell cell in m_field)
            {
                if (cell.Update())
                {
                    ++cells;
                }

            }
        }
        Debug.Log("No cells left");
    }
}
