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

    // 1st pass: random seed noise in Perlin
    for (int x=0; x<width; x++)
    {
      for (int y=0; y<width; y++)
      {
        tiles[x,y] = new Tile(.4f);//Height(x,y));
      }
    }

    // 2nd pass: Spread grass
    //SpreadGrass();
  }

  public void SpreadGrass()
  { 
    Tile[,] oldTiles = new Tile[width,width];

    for (int x=0; x<width; x++)
    {
      for (int y=0; y<width; y++)
      {
        oldTiles[x,y] = tiles[x,y];
      }
    }

    for (int x=0; x<width; x++)
    {
      for (int y=0; y<width; y++)
      {
        // count neighbors
        int neighborCount = 0;

        for (int xNeighb=-1; xNeighb<=1; xNeighb++)
        {
          for (int yNeighb=-1; yNeighb<=1; yNeighb++)
          {
            if (x+xNeighb < 0 || x+xNeighb > width-1 || y+yNeighb < 0 || y+yNeighb > width-1)
              continue;

            if (oldTiles[x+xNeighb,y+yNeighb].type == TileType.Grass)
            {
              neighborCount++;
            }
          }
        }

          // If we have >5 neighbors, become wall
          if (oldTiles[x,y].type == TileType.Grass || (oldTiles[x,y].type == TileType.None && neighborCount >= 5))
          {
            tiles[x,y].type = TileType.Grass;
          }
          if(neighborCount <= 2)
          {
            tiles[x,y].type = TileType.None;
          }

          // count empty neighbors (6)
          neighborCount = 0;

          for (int xNeighb=-1; xNeighb<=1; xNeighb++)
          {
            for (int yNeighb=0; yNeighb<1; yNeighb++)
            {
              if(tiles[x,y].type == TileType.None)
              {
              neighborCount++;
              }
            }
          if (neighborCount > 0)
          {
            tiles[x,y].border = true;
          }
      }
    }
  }
}
  //Making this a seperate function so we can add complexity more easily
  float Height(float x, float y)
  {
    return Mathf.PerlinNoise(x,y);
  }
}

public class ZoneRelationship
{
  Zone other;
  Vector3 otherDirection;   // This is used to calculate apprx. where the door to the other zone should be placed
}