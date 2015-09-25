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

public class GameManager : MonoBehaviour
{
  ZoneViewCamera zoneCameraControls;
  ZoneManager zoneManager;

  void Awake ()
  {
    zoneManager = GetComponent<ZoneManager>();
    zoneCameraControls = Camera.main.GetComponent<ZoneViewCamera>();
    BuildZone();
  }

  void BuildZone()
  {
    zoneCameraControls.Initialize();
    zoneManager.Initialize();
    //zm.BoardClear();
    //zm.BoardSetup();
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      BuildZone();
    }
  }
}