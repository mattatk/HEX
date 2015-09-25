using UnityEngine;
using System.Collections;

public class Zone {

  Tile[,] tiles;
  public int width;

  ZoneRelationship[] neighbors;

  public void Generate(int w)
  {
    width = w;
    tiles = new Tile[width, width];

    for (int x=0; x<width; x++)
    {
      for (int y=0; y<width; y++)
      {
        tiles[x,y] = new Tile(Random.Range(0,3));
      }
    }
  }
}

public class ZoneRelationship
{
  Zone other;
  Vector3 otherDirection;   // This is used to calculate apprx. where the door to the other zone should be placed
}