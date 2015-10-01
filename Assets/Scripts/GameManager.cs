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
  public static Zone currentZone;

  static GameState state;
  public static GameState State {get{return state;} set{}}

  // Cache
  public static Camera cam;
  public static ZoneViewCamera zoneCameraControls;
  public static ZoneManager zoneManager;
  public static MainUI mainUI;
  public static RoundManager roundManager;

  void Awake ()
  {
    cam =                 Camera.main;
    zoneManager =         GetComponent<ZoneManager>();
    zoneCameraControls =  Camera.main.GetComponent<ZoneViewCamera>();
    mainUI =              GetComponent<MainUI>();
    roundManager =        GetComponent<RoundManager>();

    Hex.Initialize();
    
    BuildZone();
  }

  void BuildZone()
  {
    state = GameState.ZoneMap;

    // Input

    // Network

    // Zone
    currentZone = new Zone(64);
    zoneManager.Initialize(currentZone);

    // Scene
    zoneCameraControls.Initialize();

    // Interface
    //mainUI.Initialize(); TURN BACK ON LATER
  }

    //roundManager.UpdateRound();  TURN BACK ON LATER

  void OnGUI()
  {
    //mainUI.OnMainGUI(); TURN BACK ON LATER
  }

  public static void OnTapInput(Vector2 tap)
  {
    switch (state)
    {
      case GameState.ZoneMap:
        zoneManager.OnTapInput(tap);
      break;
    }
  }
}