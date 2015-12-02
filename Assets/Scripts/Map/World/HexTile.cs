using UnityEngine;
using System.Collections;

[System.Serializable]
public class HexTile
{
  string terrainType;
  public Hexagon hexagon;

  public HexTile(Hexagon h)
  {
    hexagon = h;
  }
}
