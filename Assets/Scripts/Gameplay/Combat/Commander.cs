using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Commander
{
  public int participantID;

  public Commander(){}
  public Commander(int id)
  {
    participantID = id;
  }

  public abstract void OnWaitingForCommands();
  public void SubmitCommands(List<Command> cmds)
  {
    GameManager.combatManager.ProcessCommands(cmds);
  }
}
