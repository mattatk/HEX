using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Triangle
{
  public Vector3 v1, v2, v3, center;
  public int subdivisionLevel, index;
  public Triangle top, right, left;
  public Triangle instance, parent, childMid, childTop, childLeft, childRight;
  public TriforcePosition triforcePosition;

  //public Triangle newnx, newny, newnz;
  
  // Called for subdivisions
  public Triangle(Vector3 x, Vector3 y, Vector3 z, Triangle p, TriforcePosition tp, int sl)
  {
    v1 = x;
    v2 = y;
    v3 = z;
    center = (v1 + v2 + v3) / 3;
    instance = this;
    parent = p;
    triforcePosition = tp;
    subdivisionLevel = sl;
  }

  // Called for initial polygon
  public Triangle(Vector3 x, Vector3 y, Vector3 z)
  {
    v1 = x;
    v2 = y;
    v3 = z;
    center = (v1 + v2 + v3) / 3;
    instance = this;
    parent = null;
  }

  public void AssignNeighbors(Triangle nt, Triangle nr, Triangle nl)
  {
    top = nt;
    right = nr;
    left = nl;
  }
  
  public void AssignChildren (Triangle cm, Triangle ct, Triangle cl, Triangle cr)
  {
    childMid = cm;
    childTop = ct;
    childLeft = cl;
    childRight = cr;
  }

  public Triangle ReturnClosest(Triangle tri)
  {
    float mag1 = (tri.center - this.top.center).sqrMagnitude;
    float mag2 = (this.right.center - tri.center).sqrMagnitude;
    float mag3 = (this.left.center - tri.center).sqrMagnitude;

    if(mag1 < mag2 && mag1 < mag3)
    {
      return this.childTop;
    }
    if(mag2 < mag1 && mag2 < mag3)
    {
      return this.childRight;
    }
    if(mag3 < mag1 && mag3 < mag2)
    {
      return this.childLeft;
    }
    
    return null;
  }
  
  public Triforce OriginalToTriforce(List<Triforce> tfs)
  {
    Triforce triforce = new Triforce();
    foreach (Triforce tf in tfs)
    {
      if (tf.original == this)
      { triforce = tf; }
    }
    return triforce;
  }
  
 } 


public enum TriforcePosition{None, Mid, Top, Right, Left};

public class Triforce
{
  public Triangle original, mid, top, right, left;
  public Triforce nx, ny, nz;
  public Triforce() { }
  public Triforce(Triangle org, Triangle center, Triangle tri1, Triangle tri2, Triangle tri3)
  {
    original = org;
    mid = center;
    top = tri1;
    right = tri2;
    left = tri3;
  }
  public void AssignNeighbors(Triforce tf1, Triforce tf2, Triforce tf3)
  {
    nx = tf1;
    ny = tf2;
    nz = tf3;
  }
}
/*
public class Hexagon
{
  Vector3 center, ne, e, se, sw, w, nw;
  public Hexagon(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector3 v5, Vector3 v6)
  {
    ne = v1;
    e = v2;
    se = v3;
    sw = v4;
    w = v5;
    nw = v6;
    center = (ne + e + se + sw + w + nw) / 6;
  }
  public List<Triangle> ToRender()
  {
    List<Triangle> hexTris = new List<Triangle>();
    hexTris.Add(new Triangle(center, ne, e));
    hexTris.Add(new Triangle(center, e, se));
    hexTris.Add(new Triangle(center, se, sw));
    hexTris.Add(new Triangle(center, sw, w));
    hexTris.Add(new Triangle(center, w, nw));
    hexTris.Add(new Triangle(center, nw, ne));
    return hexTris;
  }
}
*/