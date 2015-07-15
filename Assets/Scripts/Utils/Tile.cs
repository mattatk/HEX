using UnityEngine;
using System.Collections;
using System;

[Serializable]
public abstract class Tile
{
  public Texture2D texture;
}

public class Void_Tile : Tile
{

}

public class GrassFull_Tile : Tile
{
  
}