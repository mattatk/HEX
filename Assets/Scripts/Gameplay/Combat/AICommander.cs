using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICommander : Commander
{
  public Army army;

  public AICommander(int id) : base(id){}

  public override void OnWaitingForCommands()
  {
    List<Command> commands = new List<Command>();
    commands.Add(new MoveCommand());
  }
}
