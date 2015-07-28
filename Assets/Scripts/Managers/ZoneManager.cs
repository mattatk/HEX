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
  public GameObject[] floorBottomTiles;
  public GameObject[] floorBottomBorderTiles;
	
  // === Private ===
  Transform boardHolder;
  List<Vector2> gridPositions;
  List<Vector2> topTilePositions;
  List<GameObject> tiles;
  List<GameObject> botTiles;
  float stepHeight = .08f; //proper offset of bottom tiles
  float lastStep = 0.145f; //dy from top tile to first bot tile
  bool right = false;

  public void Initialize ()
  {
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
  }
 
  public void Update()
  {
	if(Input.GetKeyDown (KeyCode.LeftArrow))
	{
	  right = false;
	  Rotate (right);
	}	
	else if (Input.GetKeyDown (KeyCode.RightArrow))
	{
	  right = true;
	  Rotate (right);
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
  }

  public void Rotate(bool right)
  {
	  /*
		Notes:
		We should rotate the grid 60 degrees, so that when we rotate we still have a vertex facing down and can use these sprites.
		We probably need to rebuild the grid when we rotate.  Not the best solution but I think we have to
		because of how we're drawing the shapes.
		So, we need to keep the data of how high up the top tiles are and how many bottom tiles need to be drawn, that means BoardSetup has to change.
		It won't look good if we do this, we'll still want to be able to show the board actually rotating to the player, 
		maybe we shouldn't do it this way at all.
		This function should probably just set up correct conditions (according to changes in BoardSetup) and call BoardSetup
	
		Right now this just rebuilds the board randomly when you press left or right arrows.
	  */
	if(right == false)
	{
	  //Do something to make the board rotate left when board setup is called
	  BoardClear ();
	  BoardSetup();
	}
	else
	{
	  //Do right
	  BoardClear ();
	  BoardSetup ();
	}
  }

  public void BoardClear() //This deletes tiles correctly but it needs to destroy the zone board game object created in BoardSetup
  {
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
  }
}