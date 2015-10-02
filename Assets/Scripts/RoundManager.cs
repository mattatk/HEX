using UnityEngine;
using System.Collections;

public class RoundManager : MonoBehaviour
{
  public Unit unitToSpawn;

  ActorSpawner actorSpawner;

  public void Initialize()
  {
    actorSpawner = GetComponent<ActorSpawner>();

    SpawnUnits();
  }

  void SpawnUnits()
  {
    actorSpawner.SpawnUnit(unitToSpawn, new IntCoord(2,2));
  }

  public void UpdateRound()
  {
    
  }

  public void OnTapInput(Vector2 tap)
  {
    /*
    RaycastHit hit;

    if (Physics.Raycast(GameManager.cam.ScreenPointToRay(tap), out hit, 500, layermask))
    {
      Vector2 hexCoordSelected = Hex.TileAt(hit.point);

      if (hexCoordSelected[0] == -1)
        return;

      Debug.Log(hexCoordSelected.x+","+hexCoordSelected.y);
    }
    */
  }
}
