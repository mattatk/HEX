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
    /*
    if (Input.GetKeyDown(KeyCode.Space))
    {
      Destroy (currentZoneObject);
    }
    if (Input.GetKeyUp(KeyCode.Space))
    {
      GameManager.currentZone.SpreadGrass();
      currentZoneObject = zoneRenderer.RenderZone(GameManager.currentZone);
    }
    */
    
  }

  public void Initialize (Zone z)
  {
    zoneRenderer = GetComponent<ZoneRenderer>();

    currentZoneObject = zoneRenderer.RenderZone(z);

    layermask = 1<<8;   // Layer 8 is set up as "Chunk" in the Tags & Layers manager

    /*
    if (gridPositions == null)
      gridPositions = new List<Vector2>();
    else
      gridPositions.Clear();

	if (topTilePositions == null)
		topTilePositions = new List<Vector2> ();
	else
		topTilePositions.Clear();

    if (tiles == null)
      tiles = new List<GameObject>();
	if (botTiles == null)
	  botTiles = new List<GameObject>();

    for (int x=0; x<columns-1; x++)
    {
      for (int y=0; y<rows; y++)
      {
        gridPositions.Add(new Vector2(x,y));
      }
    }
    */
  }



  public void BoardSetup()
  {
    /*
	 boardHolder = new GameObject("Zone Board").transform;
    Transform bt = boardHolder.transform;

    Hex hex = new Hex(hexRadius);

    for (int y=0; y<rows; y++)
    {
      for (int x=0; x<columns; x++)
      {
    		int randomHeight = Random.Range (0, 9);
    		int randomBorder = Random.Range(0, floorBorderTiles.Length);
    		int randomTiles = Random.Range (0, floorTiles.Length);
    		float topTileOffset = randomHeight*stepHeight+lastStep;
    		Vector2 tileCenter = hex.TileCenter (new Vector2(x,y));
    		//Vector2 topTile = hex.TileCenter (new Vector2(x,y+topTileOffset));
    		GameObject topInstantiate = floorTiles[randomTiles];
    		GameObject botInstantiate = floorBottomTiles[randomTiles]; //so, the top tiles and bottom tiles must have corresponding array indexes
    		
        if(x == 0 || x == columns-1 || y == 0 || y == rows-1)
    		{
          topInstantiate = floorBorderTiles [randomBorder];
    		  botInstantiate = floorBottomBorderTiles [randomBorder];
    		}
          GameObject instance = (GameObject)Instantiate (topInstantiate, tileCenter, Quaternion.identity);
          Transform t = instance.transform;
          t.Translate(0,topTileOffset,0);
          t.parent = bt;
          tiles.Add(instance);

		//Now we need to fill below the top tiles we just made.
		while(randomHeight > 0)
		{
		  GameObject botInstance = (GameObject)Instantiate (botInstantiate, tileCenter, Quaternion.identity);
		  Transform b = botInstance.transform;
		  b.Translate(0,randomHeight*stepHeight,0);
		  b.parent = t;
		  botTiles.Add (botInstance);
		  randomHeight--;
		}
				
      }
    }
    */
  }

  public void BoardClear()
  {
    /*
    if (tiles == null && botTiles == null)
      return;

    foreach (GameObject g in tiles)
    { 
	  while(g.transform.childCount != 0)
	  {
		DestroyImmediate(g.transform.GetChild(0).gameObject);
	  }
	  Destroy(g);
    }
    tiles.Clear();
    */
  }

  public void OnTapInput(Vector2 tap)
  {
    RaycastHit hit;

    if (Physics.Raycast(GameManager.cam.ScreenPointToRay(tap), out hit, 500, layermask))
    {
      Vector2 hexCoordSelected = Hex.TileAt(hit.point);

      if (hexCoordSelected[0] == -1)
        return;

      Debug.Log(hexCoordSelected.x+","+hexCoordSelected.y);
    }
  }
}