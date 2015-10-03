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

public enum GameState {None, MainMenu, GalaxyMap, WorldMap, ZoneMap};

public class GameManager : MonoBehaviour
{
  // === Public Cache ===
  public GameState beginningState = GameState.ZoneMap; //Making worlds right now, not zones. Change back to make zones.  We need to change how this works obviously. Perhaps choose in inspector?
  public static string gameSeed = "sixtynine";

  // === Public Static Cache ===
  static GameState state;
  public static GameState State {get{return state;} set{}}

  //For World/Zone
  public static World currentWorld;
  public static WorldManager worldManager;

  public static Zone currentZone;
  public static ZoneRenderer zoneRenderer;
  public static List<GameObject> currentZoneObjects;

  public static Camera cam;
  public static ZoneViewCamera zoneCameraControls;
  public static ZoneManager zoneManager;
  public static MainUI mainUI;
  public static RoundManager roundManager;

  void Awake ()
  {   
    cam =                 Camera.main;
    zoneManager =         GetComponent<ZoneManager>();
    zoneRenderer =        GetComponent<ZoneRenderer>();
    zoneCameraControls =  Camera.main.GetComponent<ZoneViewCamera>();
    mainUI =              GetComponent<MainUI>();
    roundManager =        GetComponent<RoundManager>();

    // water = (GameObject)Instantiate(water,new Vector3(0,(float)Random.Range(4,5),0),Quaternion.identity);
    Hex.Initialize();

    // Ideally, the only place state is manually set.
    state = beginningState;

    switch (state)
    {
      case GameState.WorldMap:
        BuildWorld();
      break;
      case GameState.ZoneMap:
        BuildZone();
      break;
      default:
        Debug.LogError("Please set a state in GameManager.beginningState before playing.");
      break;
    }
  }

  void BuildWorld()
  {
    worldManager = GetComponent<WorldManager>();
    currentWorld = new World(WorldSize.Small, WorldType.Verdant, Season.Spring, AxisTilt.Slight);
    worldManager.Initialize(currentWorld);

    // Round
    roundManager.Initialize();

    // Scene
    //zoneCameraControls.Initialize();
    }

  void BuildZone()
  {
    // --- Input

    // --- Network

    // --- Zone
    if (currentZoneObjects != null && currentZoneObjects.Count > 0)
    {
      foreach (GameObject g in currentZoneObjects)
        Destroy (g);
    }
    int safety = 100;
    bool buildingZone = true;
    int minimumSize = 1750;

    while (buildingZone)
    {
      currentZone = new Zone(20);

      if (currentZone.landArea > minimumSize)
      {
        Debug.Log("Zone generated with a land mass of "+currentZone.landArea+" hex.");
        buildingZone = false;
      }
      else if (currentZone.landArea>0)
      {
        Debug.Log("Land mass is too low. New level being generated....");
      }
      else
      {
        Debug.Log("Underwater level detected. New level being generated....");
      }

      safety--;
      if (safety < 0)
        break;
    }

    currentZoneObjects = zoneRenderer.RenderZone(currentZone, zoneManager.regularTileSet);
    zoneManager.Initialize(currentZone);

    // --- Round
    roundManager.Initialize();

    // --- Scene
    zoneCameraControls.Initialize();

    // --- Interface
    //mainUI.Initialize(); TURN BACK ON LATER
  }

  void Update()
  {
    if (Input.GetKeyUp(KeyCode.Space))
    {
      BuildZone();
    }
  }

  void OnGUI()
  {
    //mainUI.OnMainGUI(); TURN BACK ON LATER
  }

  public static void OnTapInput(Vector2 tap)
  {
    switch (state)
    {
      case GameState.ZoneMap:
        roundManager.OnTapInput(tap);
      break;
    }
  }
}