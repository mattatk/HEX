using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour
{
  public AICommander ai1, ai2;

  public void Initialize()
  {
    ai1 = new AICommander(1);
    ai2 = new AICommander(2);
  }

  public void BeginDuel()
  {
    ai1.OnWaitingForCommands();
    ai1.OnWaitingForCommands();
  }

  public void ProcessCommands(List<Command> cmds)
  {

  }
}

[System.Serializable]
public class Army
{
  public List<Unit> units;
}