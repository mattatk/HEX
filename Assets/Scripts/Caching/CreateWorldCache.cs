using UnityEngine;
using System.Collections;

public class CreateWorldCache : MonoBehaviour {

  public int scale = 1, subdivisions = 3;

	public void BuildCache  () {
    
    PolySphere sphere = new PolySphere(scale,subdivisions);
    World world = new World(sphere);

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
