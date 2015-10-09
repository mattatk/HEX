using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldRenderer : MonoBehaviour
{
  public int subdivisions = 1;
  public GameObject worldPrefab, centerMarkerPrefab, textMeshPrefab;
  Zone currentZone;
  public GameObject RenderWorld(World world, TileSet tileSet)
  {
    int scale = 4;

    currentZone = GameManager.currentZone;
    GameObject output = (GameObject)Instantiate(worldPrefab, Vector3.zero, Quaternion.identity);

    MeshFilter myFilter = output.GetComponent<MeshFilter>();
    MeshCollider myCollider = output.GetComponent<MeshCollider>();

    Vector3 origin = Vector3.zero;
    Vector2 uv0 = new Vector2(0, 0),
          uv1 = new Vector2(.5f, 1),
          uv2 = new Vector2(1, 0);

    Vector2 uvOffset = Vector3.zero;

    PolySphere sphere = new PolySphere(scale, subdivisions);

    //LabelCenters(sphere.finalTris);
    LabelNeighbors(sphere);

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector3> normals = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();

    foreach (Triangle tri in sphere.finalTris)
    {

    }

    // Generate polygons, create neighbors
    foreach (Triangle tri in sphere.finalTris)
    {
      // Add the verts again
      vertices.Add(tri.v1);
      normals.Add(Vector3.Normalize(origin + tri.v1));
      uvs.Add(uv1 + uvOffset);
      vertices.Add(tri.v2);
      normals.Add(Vector3.Normalize(origin + tri.v2));
      uvs.Add(uv2 + uvOffset);
      vertices.Add(tri.v3);
      normals.Add(Vector3.Normalize(origin + tri.v3));
      uvs.Add(uv2 + uvOffset);

      triangles.Add(vertices.Count - 3);
      triangles.Add(vertices.Count - 2);
      triangles.Add(vertices.Count - 1);

      //GameObject centerMarker = (GameObject)GameObject.Instantiate(centerMarkerPrefab, tri.center, Quaternion.identity);
    }

    /*
    foreach(Triangle tri in allTriangles)
    {
      //Create a zone
      Zone triZone = new Zone(tri);
    }
    */

    Mesh m = new Mesh();
    m.vertices = vertices.ToArray();
    m.triangles = triangles.ToArray();
    m.normals = normals.ToArray();
    m.uv = uvs.ToArray();

    myCollider.sharedMesh = m;
    myFilter.sharedMesh = m;

    return output;
  }

  void LabelNeighbors(PolySphere sphere)
  {
    Dictionary<Triangle, bool> neighborsLabeled = new Dictionary<Triangle, bool>();

    
  }

  void LabelCenters(List<Triangle> tris)
  {
    int count = 0;
    foreach (Triangle t in tris)
    {
      GameObject textObj = (GameObject)Instantiate(textMeshPrefab, t.center * 1.01f, Quaternion.LookRotation(-t.center));
      textObj.GetComponent<TextMesh>().text = count.ToString();

      count++;
    }

  }
}

