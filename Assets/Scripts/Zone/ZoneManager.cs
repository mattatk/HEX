/*
 * Copyright (c) 2015 Colin James Currie.
 * All rights reserved.
 * Contact: cj@cjcurrie.net
 */

 // @INFO: This script is responsible for rendering zone data and performing simulation at the zone level

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ZoneManager : MonoBehaviour
{
  // === Public ===
  public TileSet regularTileSet;

  public int columns = 8, rows = 8;

  public Count wallCount = new Count(5,9);

  public GameObject[] floorTiles;
  public GameObject[] floorBorderTiles;
  public GameObject[] floorBottomTiles;
  public GameObject[] floorBottomBorderTiles;
	
  // === Private ===
  Transform boardHolder;
  List<Vector2> gridPositions;
  List<Vector2> topTilePositions;

  // === Cache ===
  ZoneRenderer zoneRenderer;

  // Deprecated
  //List<GameObject> tiles;
  //List<GameObject> botTiles;
  //float stepHeight = .08f; //proper offset of bottom tiles
  //float lastStep = 0.145f; //dy from top tile to first bot tile
  //bool right = false;

  GameObject currentZoneObject;
  int layermask;

  void Update()
  {
    if (Input.GetKeyUp(KeyCode.Space))
    {
      Destroy (currentZoneObject);
      //GameManager.currentZone.SimulateLife();
      GameManager.currentZone = new Zone (64);
      currentZoneObject = zoneRenderer.RenderZone(GameManager.currentZone, regularTileSet);
    }
  }

  public void Initialize (Zone z)
  {
    zoneRenderer = GetComponent<ZoneRenderer>();

    currentZoneObject = zoneRenderer.RenderZone(z, regularTileSet);

    layermask = 1<<8;   // Layer 8 is set up as "Chunk" in the Tags & Layers manager
  }

  public void OnTapInput(Vector2 tap)
  {
    
  }
}