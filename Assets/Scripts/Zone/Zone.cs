using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Zone {

  public Tile[,] tiles;
  public List<Tile[,]> continents;
  public int width;

  ZoneRelationship[] neighbors;

  public Zone(){}

  public Zone(int w)
  {
    width = w;
    tiles = new Tile[width, width];

    int randX = Random.Range(-99999,99999);
    int randY = Random.Range(-99999,99999);


    // 1st pass: random seed noise in Perlin

    for (int x=0; x<width; x++)
    {
      for (int y=0; y<width; y++)
      {
        //tiles[x,y] = new Tile(.4f);   // Bay area lol
        float seedx = Random.Range(-100,100);
        float seedy = Random.Range(-100,100);
        tiles[x,y] = new Tile(x/(float)width+seedx,y/(float)width+seedy,.5f);
      }
    }

    // 2nd pass: Spread ground
    SpreadGround(4);
    //3rd: Refine ground
    RefineGround();
    // 4th: Border manipulations
    //SetBorders();
    //SpreadGround();

    SetHeightsByPerlin(.1f, 10);
    SetHeightsByPerlin(.6f, 3);
  }

  public void SimulateLife(){

  }

  void SetHeightsByPerlin(float scale, int lacunarity)
  {
    float seedx = Random.Range(-100,100);
        float seedy = Random.Range(-100,100);

    for (int x=0; x<width; x++)
    {
      for (int y=0; y<width; y++)
      {
        

        float height = (int)(Mathf.PerlinNoise((float)x/width+seedx,(float)y/width+seedy) * lacunarity) * scale;
        tiles[x,y].height += height;
      }
    }
  }

  public void SpreadGround(int iterations)
  { 
    Tile[,] oldTiles = new Tile[width,width];
    for(int i = 1; i < iterations; i++)  //Increase i bounds to iterate the grid more times
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
              if ((x+xNeighb < 0 || x+xNeighb >= width || y+yNeighb < 0 || y+yNeighb >= width) )
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
          if (oldTiles[x,y].type == TileType.Grass || neighborGroundCount >= 5)
          {
              tiles[x,y].type = TileType.Grass;
          }

          if(neighborGroundCount <= 2)
          {
              tiles[x,y].type = TileType.None;
          }
       }
     }
    }
   }
    public void RefineGround()
    {
        for (int i = 1; i < 6; i++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    int neighborGroundCount = 0;
                    int neighborEmptyCount = 0;
                    for (int xNeighb = -1; xNeighb <= 1; xNeighb++)
                    {
                        for (int yNeighb = -1; yNeighb <= 1; yNeighb++)
                        {
                            if ((x + xNeighb < 0 || x + xNeighb > width - 1 || y + yNeighb < 0 || y + yNeighb > width - 1) || (yNeighb == 1 && xNeighb == 1) || (yNeighb == -1 && xNeighb == -1))
                                continue;
                            if (tiles[x + xNeighb, y + yNeighb].type == TileType.None)
                                neighborEmptyCount++;
                            if (tiles[x + xNeighb, y + yNeighb].type != TileType.None)
                                neighborGroundCount++;
                        }
                    }
                    if (neighborGroundCount < neighborEmptyCount)
                    {
                        tiles[x, y].type = TileType.None;
                    }
                }
            }
        }

        //Now take out tiles with one or less ground neighbor.        
        int neighborCount = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                neighborCount = 0;
                for (int xNeighb = -1; xNeighb <= 1; xNeighb++)
                {
                    for (int yNeighb = -1; yNeighb <= 1; yNeighb++)
                    {
                        if (y % 2 == 0) //even
                        {
                            if ((yNeighb == 1 && xNeighb == 1) || (yNeighb == -1 && xNeighb == 1))
                            {
                                continue;
                            }
                        }
                        else //odd
                        {
                            if ((yNeighb == 1 && xNeighb == -1) || (yNeighb == -1 && xNeighb == -1))
                            {
                                continue;
                            }
                        }
                        if ((x + xNeighb < 0 || x + xNeighb > width - 1 || y + yNeighb < 0 || y + yNeighb > width - 1))
                        {
                            //tiles[x, y].type = TileType.None;
                            continue;
                        }
                        if (tiles[x + xNeighb, y + yNeighb].type == TileType.Grass)
                            neighborCount++;
                    }
                }
                if (neighborCount <= 1)
                {
                    tiles[x, y].type = TileType.None;
                }
            }
        }
    }
    public void SetBorders()
    {
        int neighborCount = 0;
 
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                // count empty neighbors (6)
                neighborCount = 0;

                for (int xNeighb = -1; xNeighb <= 1; xNeighb++)
                {
                    for (int yNeighb = -1; yNeighb <= 1; yNeighb++)
                    {
                        if ((x + xNeighb < 0 || x + xNeighb > width - 1 || y + yNeighb < 0 || y + yNeighb > width - 1))
                            continue;
                        if (y % 2 == 0) //even
                        {
                            if ((yNeighb == 1 && xNeighb == 1) || (yNeighb == -1 && xNeighb == 1))
                            {
                                continue;
                            }
                        }
                        else //odd
                        {
                            if ((yNeighb == 1 && xNeighb == -1) || (yNeighb == -1 && xNeighb == -1))
                            {
                                continue;
                            }
                        }
                      
                        if (tiles[x + xNeighb, y + yNeighb].type == TileType.None)
                        {
                            neighborCount++;
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
    //needs to make the continents between borders and put each land segment into its own array, in order to compare size later and keep the biggest one or few
    public void FillBorders()
    {
        int i = 0;
        //Get an empty tile array which we will populate the next array in continents with
        Tile[,] tilesToSave = new Tile[width, width];
       
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                if (tiles[x, y].posBorderCheck)
                    continue;
                if (tiles[x, y].border) //we're only going to do this for borders we haven't done this for yet
                {
                    RecursiveFill(x,y);
                }
            }
        }
    }

    bool walk = false;
    public void RecursiveFill(int x, int y)
    {
      walk = false;
      while (walk)
      {

      }
      for (int xNeighb = -1; xNeighb <= 1; xNeighb++)
      {
          for (int yNeighb = -1; yNeighb <= 1; yNeighb++)
          {
              if ((x + xNeighb < 0 || x + xNeighb > width - 1 || y + yNeighb < 0 || y + yNeighb > width - 1))
                  continue;
              if (y % 2 == 0) //even
              {
                  if ((yNeighb == 1 && xNeighb == 1) || (yNeighb == -1 && xNeighb == 1))
                  {
                      continue;
                  }
              }
              else //odd
              {
                  if ((yNeighb == 1 && xNeighb == -1) || (yNeighb == -1 && xNeighb == -1))
                  {
                      continue;
                  }
              }
          }
      }

      // count empty neighbors (6)
      int neighborCount = 0;

      for (int xNeighb = -1; xNeighb <= 1; xNeighb++)
      {
        for (int yNeighb = -1; yNeighb <= 1; yNeighb++)
        {
          if ((x+xNeighb < 0 || x+xNeighb > width - 1 || y + yNeighb < 0 || y + yNeighb > width - 1))
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