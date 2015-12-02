using UnityEngine;
using System.Collections;

[RequireComponent(typeof(WorldRenderer))]
public class WorldManager : MonoBehaviour
{
  // === Public ===
  public World activeWorld;
  public TileSet regularTileSet;
  public float maxMag = 10;
  // === Cache ===
  WorldRenderer worldRenderer;
  GameObject currentWorldObject;
  Transform worldTrans;
  //int layermask; @TODO: stuff

  public void Initialize()
  {
    activeWorld = LoadWorld();
    worldRenderer = GetComponent<WorldRenderer>();
    currentWorldObject = new GameObject("World");
    worldTrans = currentWorldObject.transform;

   //currentWorld = new World(WorldSize.Small, WorldType.Verdant, Season.Spring, AxisTilt.Slight);

    foreach (GameObject g in worldRenderer.RenderWorld(activeWorld, regularTileSet))
    {
      g.transform.parent = worldTrans;
    }

    //layermask = 1 << 8;   // Layer 8 is set up as "Chunk" in the Tags & Layers manager
  }

  World LoadWorld()
  {
    return BinaryHandler.ReadData<World>(World.cachePath);
  }
}
