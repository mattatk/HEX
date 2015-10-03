using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class Actor
{
  public GameObject prefab, instance;
  public Transform instanceTrans;   // Usually initialized in the ActorSpawner
}
