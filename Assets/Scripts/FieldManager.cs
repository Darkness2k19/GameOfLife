using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class FieldManager : MonoBehaviour
{
    private static FieldManager m_instance = null;

    [SerializeField]
    private Vector2Int m_fieldSize;
    [SerializeField]
    private Cell[,] m_field;

    [SerializeField]
    private TileBase m_tile;
    private Tilemap m_tilemap;

    [SerializeField]
    private TileBase m_borderTile;
    [SerializeField]
    private Tilemap m_borderTilemap;

    [SerializeField]
    private List<Color> m_colorPalette;

    [SerializeField]
    [Range(0.25f, 10f)]
    public float timeSpeed = 1f;

    private Coroutine m_cellUpdater = null;
    public bool m_isStopped = true;

    public bool isStopped
    {
        get => m_isStopped;
        set
        {
            if (m_isStopped == value)
            {
                return;
            }
            m_isStopped = value;
            if (!value && m_cellUpdater == null)
            {
                m_cellUpdater = StartCoroutine(updateCells());
            }
        }
    }

    [SerializeField]
    private Text m_pauseButtonText;

    public static FieldManager GetInstance()
    {
        return m_instance;
    }

    public int GetHolderID(Vector2Int position)
    {
        return m_field[position.x, position.y].holderId;
    }

    public bool IsEmpty(Vector2Int position)
    {
        return m_field[position.x, position.y].IsEmpty();
    }

    public void UpdateCell(Vector2Int position, int holderId)
    {
        m_field[position.x, position.y].holderId = holderId;
        m_field[position.x, position.y].Update();
    }

    private void generate()
    {
        m_field = new Cell[m_fieldSize.x, m_fieldSize.y];

        int cells = 0;
        for (int x = 0; x < m_fieldSize.x; ++x)
        {
            for (int y = 0; y < m_fieldSize.y; ++y)
            {
                m_tilemap.SetTile(new Vector3Int(x, y), m_tile);
                m_borderTilemap.SetTile(new Vector3Int(x, y), m_borderTile);

                ref Cell cell = ref m_field[x, y];
                // var randomValue = Random.Range(0, 2);
                var randomValue = 1;
                cell = new Cell(m_tilemap, new Vector2Int(x, y), randomValue > 0 ? -1 : 0, m_colorPalette);
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

    private IEnumerator updateCells()
    {
        uint cells = 1;
        while (cells > 0 && !isStopped)
        {
            yield return new WaitForSeconds(0.125f / timeSpeed);
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
        m_cellUpdater = null;
        if (cells == 0)
        {
            Debug.Log("No cells left");
        }
    }

    public void SwapStoppedState()
    {
        isStopped = !isStopped;
        if (isStopped)
        {
            m_pauseButtonText.text = "Continue";
        }
        else
        {
            m_pauseButtonText.text = "Pause";
        }
    }

    void Awake()
    {
        if (m_instance != null)
        {
            Debug.LogError("Attempt to create second singleton of type FieldManager");
            return;
        }

        m_instance = this;
    }

    void Start()
    {
        m_tilemap = GetComponent<Tilemap>();
        generate();

        isStopped = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwapStoppedState();
        }
    }
}
