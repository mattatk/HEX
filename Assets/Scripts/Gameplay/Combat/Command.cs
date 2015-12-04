using UnityEngine;
using System.Collections;

public abstract class Command{}

public class MoveCommand : Command
{
  public HexTile target;
  public MoveCommand()
  {
    //HexTile targ
    //target = targ;
  }
}