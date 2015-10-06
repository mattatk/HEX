using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class Actor
{
  public GameObject prefab;
  [HideInInspector] public GameObject instance;
  [HideInInspector] public Transform instanceTrans;   // Usually initialized in the ActorSpawner
}
