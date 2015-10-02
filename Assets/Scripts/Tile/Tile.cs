using UnityEngine;
using System;
using System.Collections;

public enum TileType {None, Grass, GrassSparse, Rock, Hill, Desert, Water, Border};

[Serializable]
public class Tile
{
  public float height;

  public bool border;
  public bool posBorderCheck= false;

  public TileType type;

  public Tile(){}

  public Tile(float probability)    // AKA "bay area"
  {
    if (UnityEngine.Random.Range(0, 1.0f) < probability)
    {
      type = TileType.Grass;
    }
    else
      type = TileType.None;

    height = 0;
  }
  
  public Tile(float x, float y, float probability)
  {
    if (Mathf.PerlinNoise((float)x,(float)y) < probability)
    {
      type = TileType.Grass;
    }
    else
      type = TileType.None;

    height = 0;//Mathf.PerlinNoise((float)x,(float)y);
  }

  public virtual void OnUnitEnter(){}
}

public class Tile_Grass : Tile
{ 
  public override void OnUnitEnter()
  {
    Debug.Log("The grass rustles as a unit enters.");
    // Some custom tile logic here
  }
}