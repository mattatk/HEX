using UnityEngine;
using System.Collections;

public class CreateWorldCache : MonoBehaviour {

  public int scale = 10, subdivisions = 3;

	public void BuildCache  (World world) 
  {
    world.PrepForCache(scale, subdivisions);

    try
    {
      BinaryHandler.WriteData<World>(world, World.cachePath);
      Debug.Log ("World cache concluded.");
    }
    catch(System.Exception e)
    {
      Debug.LogError ("World cache fail: "+e);
    }
	}
	
}
