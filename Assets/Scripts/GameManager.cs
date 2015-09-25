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
  GameState state;
  public GameState State {get{return state;} set{}}

  // Cache
  ZoneViewCamera zoneCameraControls;
  ZoneManager zoneManager;
  MainUI mainUI;
  RoundManager roundManager;

  void Awake ()
  {
    zoneManager =         GetComponent<ZoneManager>();
    zoneCameraControls =  Camera.main.GetComponent<ZoneViewCamera>();
    mainUI =              GetComponent<MainUI>();
    roundManager =        GetComponent<RoundManager>();

    BuildZone();
  }

  void BuildZone()
  {
    state = GameState.ZoneMap;

    // Input

    // Network

    // Scene
    zoneCameraControls.Initialize();
    zoneManager.Initialize();


    // Interface
    mainUI.Initialize();
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      BuildZone();
    }

    roundManager.UpdateRound();
  }

  void OnGUI()
  {
    mainUI.OnMainGUI();
  }
}