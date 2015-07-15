/*
 * Copyright (c) 2015 Colin James Currie.
 * All rights reserved.
 * Contact: cj@cjcurrie.net
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ZoneManager : MonoBehaviour
{
  [Serializable]
  public class Count
  {
    public int minimum, maximum;

    public Count(int min, int max)
    {
      maximum = max;
      minimum = min;
    }
  }

  // === Public ===

  public int columns = 8, rows = 8;

  public float hexRadius = .3f;

  public Count wallCount = new Count(5,9);

  public GameObject[] floorTiles;
  public GameObject[] floorBorderTiles;

  // === Private ===
  Transform boardHolder;
  List<Vector2> gridPositions;
  List<GameObject> tiles;
  float stepHeight = .2f;

  public void Initialize ()
  {
    if (gridPositions == null)
      gridPositions = new List<Vector2>();
    else
      gridPositions.Clear();

    if (tiles == null)
      tiles = new List<GameObject>();

    for (int x=0; x<columns-1; x++)
    {
      for (int y=0; y<rows; y++)
      {
        gridPositions.Add(new Vector2(x,y));
      }
    }
  }

  public void BoardSetup()
  {
    boardHolder = new GameObject("Zone Board").transform;
    Transform bt = boardHolder.transform;

    Hex hex = new Hex(hexRadius);

    for (int y=0; y<rows; y++)
    {
      for (int x=0; x<columns; x++)
      {
        GameObject toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];

        if(x == 0 || x == columns-1 || y == 0 || y == rows-1)
          toInstantiate = floorBorderTiles [Random.Range (0, floorBorderTiles.Length)];

        GameObject instance = (GameObject)Instantiate (toInstantiate, hex.TileCenter(new Vector2(x,y)), Quaternion.identity);
        Transform t = instance.transform;
        t.Translate(0,Random.Range(0,2)*stepHeight,0);
        t.parent = bt;
        tiles.Add(instance);
      }
    }
  }

  public void BoardClear()
  {
    if (tiles == null)
      return;

    foreach (GameObject g in tiles)
    {
      Destroy(g);
    }

    tiles.Clear();
  }
}