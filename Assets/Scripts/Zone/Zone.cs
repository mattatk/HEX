using UnityEngine;
using System.Collections;

public class Zone {

  public Tile[,] tiles;
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
        tiles[x,y] = new Tile(Height(x,y));
        
      }
    }
  }

  //Making this a seperate function so we can add complexity more easily
  float Height(float x, float y)
  {
    return (0.001f * (Mathf.Pow(x, 2) + Mathf.Pow(y, 2)));
  }
}

public class ZoneRelationship
{
  Zone other;
  Vector3 otherDirection;   // This is used to calculate apprx. where the door to the other zone should be placed
}