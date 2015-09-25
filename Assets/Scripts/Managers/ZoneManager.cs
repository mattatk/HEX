﻿/*
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
  public GameObject boardPrefab;

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

  GameObject currentZoneObject;
  public Zone currentZone;

  public void Initialize ()
  {
    currentZone = new Zone();
    currentZone.Generate(64);

    currentZoneObject = RenderZone(currentZone);

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

  GameObject RenderZone(Zone zone)
  {
    float side = .25f;   // Main scaling unit for the hexagon

    float root3 = Mathf.Sqrt(3);
    float halfSide = side/2;
    float height = root3 * side / 2;
    float doubleHeight = height*2;
    float sideAndAHalf = halfSide * 3;

    Vector3 zonePlacement = new Vector3(-1*height*zone.width, 0, sideAndAHalf*zone.width/-2);
    GameObject output = (GameObject)Instantiate(boardPrefab, zonePlacement, Quaternion.identity);

    Renderer myRend = output.GetComponent<Renderer>();
    MeshFilter myFilter = output.GetComponent<MeshFilter>();
    MeshCollider myCollider = output.GetComponent<MeshCollider>();

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector3> normals = new List<Vector3>();
    List<Vector2>uvs = new List<Vector2>();

    
    Vector3 origin,
                v1 = new Vector3(0,0,side),
                v2 = new Vector3(height, 0, halfSide),
                v3 = new Vector3(height, 0, -halfSide),
                v4 = new Vector3(0,0,-side),
                v5 = new Vector3(-height,0,-halfSide),
                v6 = new Vector3(-height, 0, halfSide);

    float texHeight = 868;
    float texWidth = 1006;            
    Vector2 uv0 = new Vector2(503/texWidth,434/texHeight),
            uv1 = new Vector2(754/texWidth, 1),
            uv2 = new Vector2(1,.5f),
            uv3 = new Vector2(uv1.x, 0),
            uv4 = new Vector2(252/texWidth,0),
            uv5 = new Vector2(0,.5f),
            uv6 = new Vector2(uv4.x,1);

    int counter = 0;

    for (int x=0; x<zone.width; x++)
    {
      for (int y=0;y<zone.width; y++)
      {
        /*
              1 = 0,side
          6         2=height, side/2
           origin(0)
          5         3=height, -side/2
              4= 0,-side
        */

        float xOffset = x*doubleHeight;

        if (y%2==1)
          xOffset += height;
        origin = new Vector3(xOffset, 0, y*sideAndAHalf);

        // Add the first hexagon
        vertices.Add(origin);
        normals.Add(Vector3.up);
        uvs.Add(uv0);

        vertices.Add(origin+v1);
        normals.Add(Vector3.up);
        uvs.Add(uv1);

        vertices.Add(origin+v2);
        normals.Add(Vector3.up);
        uvs.Add(uv2);

        vertices.Add(origin+v3);
        normals.Add(Vector3.up);
        uvs.Add(uv3);

        vertices.Add(origin+v4);
        normals.Add(Vector3.up);
        uvs.Add(uv4);

        vertices.Add(origin+v5);
        normals.Add(Vector3.up);
        uvs.Add(uv5);

        vertices.Add(origin+v6);
        normals.Add(Vector3.up);
        uvs.Add(uv6);
        /*
              .....
             / 6|  /\
            / \ |1/  \
           / 5 \|/  2 \
           \  /4|3\  /
            \/..|..\/
        */

        // Triangle 1
        triangles.Add(counter);
        triangles.Add(counter+1);
        triangles.Add(counter+2);
        // Triangle 2
        triangles.Add(counter);
        triangles.Add(counter+2);
        triangles.Add(counter+3);
        // Triangle 3
        triangles.Add(counter);
        triangles.Add(counter+3);
        triangles.Add(counter+4);
        // Triangle 4
        triangles.Add(counter);
        triangles.Add(counter+4);
        triangles.Add(counter+5);
        // Triangle 5
        triangles.Add(counter);
        triangles.Add(counter+5);
        triangles.Add(counter+6);
        // Triangle 6
        triangles.Add(counter);
        triangles.Add(counter+6);
        triangles.Add(counter+1);

        counter += 7;
      }
    }


    Mesh m = new Mesh();
    m.vertices = vertices.ToArray();
    m.triangles = triangles.ToArray();
    m.normals = normals.ToArray();
    m.uv = uvs.ToArray();

    myCollider.sharedMesh = m;
    myFilter.sharedMesh = m;

    return output;
  }
 
  public void Update()
  {
    /*
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
    */
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