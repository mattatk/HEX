using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldRenderer : MonoBehaviour
{
  public int subdivisions = 1;
  public GameObject worldPrefab, textMeshPrefab;
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

    LabelCenters(sphere.finalTris);
    LabelNeighbors(sphere);

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector3> normals = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();

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
    /*
    Dictionary<Triangle, bool> neighborsLabeled = new Dictionary<Triangle, bool>();

    foreach (Triangle tri in sphere.finalTris)
    {
      if (neighborsLabeled.ContainsKey(tri) )
        continue;

      neighborsLabeled.Add(tri.instance, true);

      // Do the three immediate neighbors
      if (!neighborsLabeled.ContainsKey(tri.top))
      {
        //neighborsLabeled.Add(tri.top.instance, true);

        Vector3 midPointNX = (tri.center+tri.top.center) / 2;
        GameObject textObj = (GameObject)Instantiate(textMeshPrefab, midPointNX * 1.03f,
                                Quaternion.LookRotation(-midPointNX, tri.center-tri.top.center));
        textObj.GetComponent<TextMesh>().text = "|";
      }

      if (!neighborsLabeled.ContainsKey(tri.right))
      {
        //neighborsLabeled.Add(tri.right.instance, true);

        Vector3 midPointNY = (tri.center+tri.right.center) / 2;
        GameObject textObj = (GameObject)Instantiate(textMeshPrefab, midPointNY * 1.03f,
                                Quaternion.LookRotation(-midPointNY, tri.center-tri.right.center));
        textObj.GetComponent<TextMesh>().text = "|";
      }

      if (!neighborsLabeled.ContainsKey(tri.left))
      {
        //neighborsLabeled.Add(tri.left.instance, true);

        Vector3 midPointNZ = (tri.center+tri.left.center) / 2;
        GameObject textObj = (GameObject)Instantiate(textMeshPrefab, midPointNZ * 1.03f,
                                Quaternion.LookRotation(-midPointNZ, tri.center-tri.left.center));
        textObj.GetComponent<TextMesh>().text = "|";
      }
    }
    */

    foreach (Triangle tri in sphere.finalTris)
    {
      float scale = tri.subdivisionLevel>0?1.055f : 1.36f;

      Vector3 midPointTop = (tri.center+tri.top.center) / 2,
              midPointRight = (tri.center+tri.right.center) / 2,
              midPointLeft = (tri.center+tri.left.center) / 2;

      GameObject textObj = (GameObject)Instantiate(textMeshPrefab, midPointTop * scale,
                              Quaternion.LookRotation(-midPointTop, tri.center-tri.top.center));
      textObj.GetComponent<TextMesh>().text = "|";

      Instantiate(textObj, midPointRight * scale, Quaternion.LookRotation(-midPointRight, tri.center - tri.right.center));
      Instantiate(textObj, midPointLeft * scale, Quaternion.LookRotation(-midPointLeft, tri.center-tri.left.center));
    }

  }

  void LabelCenters(List<Triangle> tris)
  {
    foreach (Triangle t in tris)
    {
      GameObject textObj = (GameObject)Instantiate(textMeshPrefab, t.center * 1.01f, Quaternion.LookRotation(-t.center));
      textObj.name = "Face "+t.index;
      textObj.GetComponent<TextMesh>().text = t.index.ToString();
    }
  }
}

