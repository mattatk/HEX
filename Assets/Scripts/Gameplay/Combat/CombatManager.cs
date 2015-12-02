using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour
{
  public void Initialize()
  {
    
  }

  public void BeginDuel()
  {

  }

}

public class CombatParticipant
{
  public Army army;
}

[System.Serializable]
public class Army
{
  public List<Unit> units;
}