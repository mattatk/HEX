using UnityEngine;
using System.Collections;

[System.Serializable]
public class HexTile
{
  public int index;
  string terrainType;
  public Hexagon hexagon;

  public HexTile(Hexagon h)
  {
    index = h.index;
    hexagon = h;
  }

  public int GetNeighborID(Direction dir)
  {
    return hexagon.neighbors[(int)dir];
  }
}