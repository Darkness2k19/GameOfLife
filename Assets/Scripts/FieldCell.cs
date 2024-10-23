using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Cell
{
  [SerializeField]
  private int m_holderId = -1;

  private int m_nextHolderId = -1;

  private readonly Tilemap m_tilemap = null;
  private readonly Vector2Int m_location;

  private readonly List<Color> m_colorPalette;

  public Cell(Tilemap tilemap, Vector2Int location, int holderId, List<Color> colorPalette)
  {
    m_tilemap = tilemap;
    m_location = location;
    m_holderId = holderId;
    m_nextHolderId = holderId;
    m_colorPalette = colorPalette;

    m_tilemap.SetTileFlags((Vector3Int)m_location, TileFlags.None);
    updateColor();
  }

  private void updateColor()
  {
    m_tilemap.SetColor((Vector3Int)m_location, m_colorPalette[m_holderId + 1]);
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
      updateColor();
    }
    return !IsEmpty();
  }

}