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


public class NewBehaviourScript : MonoBehaviour
{ 
  void Update ()
  {
    Debug.Log(Input.GetAxis("Mouse X"));
  }
}
