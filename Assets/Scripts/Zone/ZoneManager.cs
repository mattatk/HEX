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
	
  // === Private ===
  Transform boardHolder;
  List<Vector2> gridPositions;
  List<Vector2> topTilePositions;

  // === Cache ===
  ZoneRenderer zoneRenderer;

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