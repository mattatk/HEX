using UnityEngine;
using System;
using System.Collections;

public class Util
{
  public static int[] SquareToHexCoordinate(Vector2 square)
  {
    int[] hex = new int[2];

    // Find the row and column of the box that the point falls in.
    hex[1] = (int) (square.y / Hex.gridHeight);
    int column;

    bool rowIsOdd = hex[1] % 2 == 1;

    // Is the row an odd number?
    if (rowIsOdd)// Yes: Offset x to match the indent of the row
      hex[0] = (int) ((square.x - Hex.bisect) / Hex.gridWidth);
    else// No: Calculate normally
      hex[0] = (int) (square.x / Hex.gridWidth);

    return new int[]{0,0};
  }
}

[Serializable]
public class Count
{
  public int minimum, maximum;

  public Count(int min, int max)
  {
    maximum = max;
    minimum = min;
  }
}

[Serializable]
public struct IntCoord
{
  public int x,y;
  public IntCoord(int a, int b)
  {
    x=a;
    y=b;
  }
}

[Serializable]
public class Line
{
  public int x1, y1, x2, y2;

  public Line(int xa, int ya, int xb, int yb)
  {
    /*
    // Always sort the ends of a line so that x1<x2, or if x1=x2, then y1<y2
    if (xa>xb)
    {
      x1=xb;y1=yb;x2=xa;y2=ya;
    }
    else if (xa==xb)
    {
      if(ya>yb)
      {
        x1=xb;y1=yb;
        x2=xa;y2=ya;
      }
      else
      {
        x1=xa;y1=ya;
        x2=xb;y2=yb;
      }
    }
    else
    {
      // Do not perform any swap
      x1 = xa; y1 = ya; x2 = xb; y2 = yb;
    }
    */

    x1 = xa; y1 = ya; x2 = xb; y2 = yb;
  }

  public override bool Equals(object ob)
  {
    if (ob is Line)
      return LinesEqual(this, (Line)ob);
    else
      return false;
  }

  public static bool LinesEqual(Line l1, Line l2) 
  {
    try 
    {
      // L1 p1 matches L2 p1 and L1 p2 mtches L2 p2
      if (( l1.x1==l2.x1 && l1.y1==l2.y1 ) && (l1.x2==l2.x2 && l1.y2==l2.y2))
        return true;
      // L1 p1 matches L2 p2 and L1 p2 mtches L2 p1 (reflection)
      else if (( l1.x1==l2.x2 && l1.y1==l2.y2 ) && (l1.x2==l2.x1 && l1.y2==l2.y1))
        return true;
      else
        return false;
    }
    catch
    {
     return false;
    }
 }

  public override int GetHashCode()
  {
    // Note that hash tables won't work properly with this implementation
    return 1234567890; 
  }
}