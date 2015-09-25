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

public enum Direction
{
  NorthEast,
  East,
  SouthEast,
  SouthWest,
  West,
  NorthWest,
  NumberOfDirections
}

public class Hex
{
  private float radius;
  private float width;
  private float halfWidth;
  private float height;
  private float rowHeight;

  public Hex(float radius)
  {
    this.radius = radius;
    this.height = 2 * radius;
    this.rowHeight = 1.3f * radius;
    this.halfWidth = (float)Mathf.Sqrt((radius * radius) - ((radius / 2) * (radius / 2)));
    this.width = 2 * this.halfWidth;
  }

  public Vector3 TileOrigin(Vector2 tileCoordinate)
  {
    return new Vector3(
              (tileCoordinate.x * width) + ((tileCoordinate.y % 2 == 1) ? halfWidth : 0),
              tileCoordinate.y * rowHeight,
              0f);
  }

  public Vector3 TileCenter(Vector2 tileCoordinate)
  {
    return TileOrigin(tileCoordinate) + new Vector3(halfWidth, height/2, 0f);
  }

  public static Direction RotateDirection(Direction direction, int amount)
  {
    //Let's make sure our directions stay within the enumerated values.
    if (direction < Direction.NorthEast ||
        direction > Direction.NorthWest ||
        Mathf.Abs(amount) > (int)Direction.NorthWest)
    {
        throw new InvalidOperationException("Directions out of range.");
    }
   direction += amount;
   //Now we need to make sure direction stays within the proper range.
   //C# does not allow modulus operations on enums, so we have to convert to and from int.

   int n_dir = (int)direction % (int)Direction.NumberOfDirections;

   if (n_dir < 0) n_dir = (int)Direction.NumberOfDirections + n_dir;
       direction = (Direction)n_dir;

   return direction;
  }

  public static Direction Opposite(Direction direction)
  {
    return RotateDirection(direction, 3);
  }

  public static Vector2 Neighbor(Vector2 tile, Direction direction)
  {
    if (tile.y % 2 == 0) //Is this row even?
    {
      switch(direction)
      {
        case Direction.NorthEast : tile.y += 1; break;
        case Direction.East : tile.x += 1; break;
        case Direction.SouthEast: tile.y -= 1; break;
        case Direction.SouthWest: tile.y -= 1; tile.x -= 1; break;
        case Direction.West: tile.x -= 1; break;
        case Direction.NorthWest: tile.x -= 1; tile.y += 1; break;
        default: throw new InvalidOperationException("Invalid direction");
      }
    }
    else //This is an odd row.
    {
      switch (direction)
      {
        case Direction.NorthEast: tile.x += 1;  tile.y += 1; break;
        case Direction.East: tile.x += 1; break;
        case Direction.SouthEast: tile.x += 1; tile.y -= 1; break;
        case Direction.SouthWest: tile.y -= 1;; break;
        case Direction.West: tile.x -= 1; break;
        case Direction.NorthWest: tile.y += 1; break;
        default: throw new InvalidOperationException("Invalid direction");
      }
    }

    return tile;
  }

  public Vector2 TileAt(Vector3 worldCoordinate)
  {
    float rise = height - rowHeight;
    float slope = rise / halfWidth;

    int X = (int)Math.Floor(worldCoordinate.x / width);
    int Y = (int)Math.Floor(worldCoordinate.y / rowHeight);

    Vector2 offset = new Vector2(worldCoordinate.x - X * width, worldCoordinate.y - Y * rowHeight);

    if (Y % 2 == 0) //Is this an even row?
    {
      //Section type A
      //Point is below left line; inside SouthWest neighbor
      if (offset.y < (-slope * offset.x + rise))
      {
        X -= 1;
        Y -= 1;
      }
      //Point is below right line; inside SouthEast neighbor
      else if (offset.y < (slope * offset.x - rise))
      {
        Y -= 1;
      }
    }
    else
    {
      //Section type B
      if (offset.x >= halfWidth) //Is the point on the right side?
      {
          if (offset.y < (-slope * offset.x + rise * 2.0f))
        //Point is below bottom line; inside SouthWest neighbor.
              Y -= 1;
      }
      else //Point is on the left side
      {
        if (offset.x < (slope * offset.y))
          //Point is below the bottom line; inside SouthWest neighbor.
          Y -= 1;
        else //Point is above the bottom line; inside West neighbor.
          X -= 1;
      }
    }

    return new Vector2(X, Y);
  }
}