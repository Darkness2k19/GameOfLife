using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
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

    [SerializeField]
    private Text playerText;
    [SerializeField]
    private Text finishText;

    [SerializeField]
    private Text player0ScoreText;
    [SerializeField]
    private Text player1ScoreText;

    private Coroutine m_cellUpdater = null;
    public bool m_isStopped = true;

    private int m_activePlayer = 0;

    private bool m_isEmpty = false;

    private int[] m_score = new int[2];

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
            TryRestart();
        }
    }

    private void TryRestart()
    {
        if (!m_isStopped && m_cellUpdater == null)
        {
            m_cellUpdater = StartCoroutine(updateCells());
        }
    }

    public void Finish()
    {
        m_activePlayer = -1;
        m_isStopped = true;
        playerText.text = "Stop!";

        finishText.gameObject.SetActive(true);
        if (m_score[0] > m_score[1])
        {
            finishText.text = "Player0 won!";
        }
        else if (m_score[0] < m_score[1])
        {
            finishText.text = "Player1 won!";
        }
        else
        {
            finishText.text = "Draw!";
        }
    }

    public bool IsFinished()
    {
        return m_activePlayer == -1;
    }

    public static FieldManager GetInstance()
    {
        return m_instance;
    }

    public Vector2Int GetBounds()
    {
        return m_fieldSize;
    }

    public int GetActivePlayer()
    {
        return m_activePlayer;
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
        if (m_isEmpty && !m_field[position.x, position.y].IsEmpty())
        {
            TryRestart();
        }
    }

    private void generate()
    {
        m_field = new Cell[m_fieldSize.x, m_fieldSize.y];

        for (int x = 0; x < m_fieldSize.x; ++x)
        {
            m_borderTilemap.SetTile(new Vector3Int(x, -1), m_tile);
            m_borderTilemap.SetTile(new Vector3Int(x, m_fieldSize.y), m_tile);

            for (int y = 0; y < m_fieldSize.y; ++y)
            {
                m_tilemap.SetTile(new Vector3Int(x, y), m_tile);
                m_borderTilemap.SetTile(new Vector3Int(x, y), m_borderTile);

                ref Cell cell = ref m_field[x, y];
                cell = new Cell(m_tilemap, new Vector2Int(x, y), -1, m_colorPalette);
            }
        }
        for (int y = 0; y < m_fieldSize.y; ++y)
        {
            m_borderTilemap.SetTile(new Vector3Int(-1, y), m_tile);
            m_borderTilemap.SetTile(new Vector3Int(m_fieldSize.x, y), m_tile);
        }
    }

    private void updateCell(Vector2Int position)
    {
        int neighbours = 0;
        int[] colors = new int[2];
        colors[0] = 0;
        colors[1] = 0;

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
                    ++colors[m_field[x, y].holderId];
                }
            }
        }
        if (m_field[position.x, position.y].IsEmpty())
        {
            if (neighbours == 3)
            {
                int result = colors[0] > colors[1] ? 0 : 1;
                if (m_field[position.x, position.y].holderId != result)
                {
                    m_field[position.x, position.y].holderId = result;
                    ++m_score[result];
                }
            }
        }
        else
        {
            if (neighbours != 2 && neighbours != 3)
            {
                m_field[position.x, position.y].holderId = -1;
            }
            else if (colors[0] != colors[1])
            {
                int result = colors[0] > colors[1] ? 0 : 1;
                if (m_field[position.x, position.y].holderId != result)
                {
                    m_field[position.x, position.y].holderId = result;
                    ++m_score[result];
                }
            }
        }
    }

    private IEnumerator updateCells()
    {
        uint cells = 1;
        while (cells > 0 && !isStopped && !IsFinished())
        {
            m_isEmpty = false;
            yield return new WaitForSeconds(0.125f / timeSpeed);
            for (int x = 0; x < m_fieldSize.x; ++x)
            {
                for (int y = 0; y < m_fieldSize.y; ++y)
                {
                    updateCell(new Vector2Int(x, y));
                }
            }
            player0ScoreText.text = string.Format("Player0: {0}", m_score[0]);
            player1ScoreText.text = string.Format("Player1: {0}", m_score[1]);

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
            m_isEmpty = true;
        }
    }

    public void SwitchPlayer()
    {
        if (IsFinished())
        {
            return;
        }
        m_activePlayer = 1 - m_activePlayer;
        playerText.text = string.Format("Player: {0}", m_activePlayer);
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
}
