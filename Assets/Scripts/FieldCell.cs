using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Cell
{
  [SerializeField]
  private int m_holderId = -1;

  private int m_nextHolderId = -1;

  private readonly Tilemap m_tilemap = null;
  public readonly Vector2Int m_location;

  public Cell(Tilemap tilemap, Vector2Int location, int holderId)
  {
    m_tilemap = tilemap;
    m_location = location;
    m_holderId = holderId;
    m_nextHolderId = holderId;

    m_tilemap.SetTileFlags((Vector3Int)m_location, TileFlags.None);
    m_tilemap.SetColor((Vector3Int)m_location, IsEmpty() ? Color.green : Color.red);
  }

  public bool IsEmpty()
  {
    return m_holderId == -1;
  }

  public int holderId
  {
    get => m_holderId;
    set
    {
      m_nextHolderId = value;
    }
  }

  public bool Update()
  {
    if (m_nextHolderId != m_holderId)
    {
      m_holderId = m_nextHolderId;
      m_tilemap.SetColor((Vector3Int)m_location, IsEmpty() ? Color.green : Color.red);
    }
    return !IsEmpty();
  }

}