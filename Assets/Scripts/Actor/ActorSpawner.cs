using UnityEngine;
using System.Collections;

public class ActorSpawner : MonoBehaviour
{
  public GameObject SpawnUnit(Actor a, Unit u, IntCoord coord)
  {
    GameObject output = (GameObject)Instantiate (a.prefab, Hex.TileOrigin(coord), Quaternion.identity);
    a.instance = output;
    a.instanceTrans = output.transform;
    
    return output;
  }
}
