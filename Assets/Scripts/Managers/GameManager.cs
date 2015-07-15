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
  ZoneManager zm;

  void Awake ()
  {
    zm = (ZoneManager)(GetComponent<ZoneManager>());

    BuildZone();
  }

  void BuildZone()
  {
    zm.Initialize();
    zm.BoardClear();
    zm.BoardSetup();
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      BuildZone();
    }
  }
}