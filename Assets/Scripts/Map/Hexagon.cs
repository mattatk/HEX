﻿using UnityEngine;
using System.Collections;

public enum Direction
{
  Y,      // 0, 1
  XY,     // 1, 1
  X,      // 1, 0
  NegY,   // 0, -1
  NegXY,  // -1, -1
  NegX,   // -1, 0
  Count
}

[System.Serializable]
public class Hexagon
{
  public int index;
  public SerializableVector3 center, v1, v2, v3, v4, v5, v6;
  public int[] neighbors;

  public Hexagon(int i, Vector3 c, Vector3[] verts)
  {
    index = i;
    neighbors = new int[]{-1,-1,-1,-1,-1,-1};
    center = c;
    v1 = verts[0];
    v2 = verts[1];
    v3 = verts[2];
    v4 = verts[3];
    v5 = verts[4];
    v6 = verts[5];
  }
}