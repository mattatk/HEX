﻿using UnityEngine;
using System.Collections;

public class ActorSpawner : MonoBehaviour
{
  public GameObject SpawnUnit(Unit u, IntCoord coord)
  {
    GameObject output = (GameObject)Instantiate (u.prefab, Hex.TileOrigin(coord), Quaternion.identity);
    u.instance = output;
    u.instanceTrans = output.transform;
    
    return output;
  }
}
