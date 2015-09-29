using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Zone {

  public Tile[,] tiles;
  public List<Tile[,]> bordersList;
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

    // 2nd pass: Spread ground
    SpreadGround();
  }

  public void SpreadGround()
  { 
    Tile[,] oldTiles = new Tile[width,width];
    for(int i = 1; i < 3; i++)  //Increase i bounds to iterate the grid more times
    {
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
        int neighborGroundCount = 0;
        int neighborEmptyCount = 0;
        for (int xNeighb=-1; xNeighb<=1; xNeighb++)
        {
          for (int yNeighb=-1; yNeighb<=1; yNeighb++)
          {
            if ((x+xNeighb < 0 || x+xNeighb > width-1 || y+yNeighb < 0 || y+yNeighb > width-1) || (yNeighb == 1 && xNeighb == 1) || (yNeighb == -1 && xNeighb == -1) )
              continue;

            if (oldTiles[x+xNeighb,y+yNeighb].type != TileType.None)
            {
              neighborGroundCount++;
            }
            if (oldTiles[x + xNeighb, y + yNeighb].type == TileType.None)
            {
                neighborEmptyCount++;
            }
          }
        }
        //How about this: if your neighbor grass count is higher than neighbor empty count, become a grass  
        // If we have >5 neighbors, become wall
        if (oldTiles[x,y].type == TileType.Grass || neighborGroundCount >= 3)
        {
            tiles[x,y].type = TileType.Grass;
        }

        if(neighborGroundCount <= 2)
        {
            tiles[x,y].type = TileType.None;
        }

        // count empty neighbors (6)
        int neighborCount = 0;

        for (int xNeighb = -1; xNeighb <= 1; xNeighb++)
        {
            for (int yNeighb = -1; yNeighb <= 1; yNeighb++)
            {
                if ((x + xNeighb < 0 || x + xNeighb > width - 1 || y + yNeighb < 0 || y + yNeighb > width - 1) || (yNeighb == 1 && xNeighb == 1) || (yNeighb == -1 && xNeighb == -1))
                    continue;
                if (tiles[x+xNeighb, y+yNeighb].type == TileType.None)
                {
                    neighborCount++;
                }
                if (tiles[x + xNeighb, y + yNeighb].border == true && !(tiles[x + xNeighb, y + yNeighb].type == TileType.None))
                {
                    
                }
            }   
        }
        if (neighborCount > 0)
        {
            tiles[x, y].border = true;
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