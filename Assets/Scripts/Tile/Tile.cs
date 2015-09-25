using UnityEngine;
using System;
using System.Collections;

public enum TileType {None, Grass, Hill};

[Serializable]
public class Tile
{
  public int height;

  public TileType type;

  public Tile(){}

  public Tile(int h)
  {
    height = h;
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