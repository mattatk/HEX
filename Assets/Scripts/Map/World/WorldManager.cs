using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour {

    // === Public ===
    public TileSet regularTileSet;
    public float maxMag = 10;
    // === Cache ===
    WorldRenderer worldRenderer;
    GameObject currentWorldObject;
    int layermask;

    public void Initialize(World z)
    {
        worldRenderer = GetComponent<WorldRenderer>();

        currentWorldObject = worldRenderer.RenderWorld(z, regularTileSet);

        layermask = 1 << 8;   // Layer 8 is set up as "Chunk" in the Tags & Layers manager
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
