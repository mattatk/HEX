using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldRenderer : MonoBehaviour
{
  public int subdivisions = 1;
  public int scale = 1;
  public int i = 0;
  public GameObject worldPrefab, textMeshPrefab;
  //Zone currentZone;
  public bool controlx;
  public bool controly;
  public bool controlz;
  public GameObject RenderWorld(World world, TileSet tileSet)
  {
    return RecursiveRender(world, tileSet, controlx, controly, controlz);
  }
  public GameObject RecursiveRender(World world, TileSet tileSet, bool cx, bool cy, bool cz)
  {
    //currentZone = GameManager.currentZone;
    GameObject output = (GameObject)Instantiate(worldPrefab, Vector3.zero, Quaternion.identity);

    MeshFilter myFilter = output.GetComponent<MeshFilter>();
    MeshCollider myCollider = output.GetComponent<MeshCollider>();

    Vector3 origin = Vector3.zero;
    Vector2 uv0 = Vector2.zero,
          uv1 = new Vector2(.5f, 1),
          uv2 = new Vector2(1, 0);

    Vector2 uvOffset = Vector3.zero;

    PolySphere sphere = new PolySphere(scale, subdivisions);

    //LabelCenters(sphere.finalTris);
    //LabelNeighbors(sphere);

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector3> normals = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();

    //Generate quadrant
    foreach (SphereTile st in sphere.sTiles)
    {
      if (ControlX(st.center.x) && ControlY(st.center.y) && ControlZ(st.center.z))
      {
        foreach (Triangle tri in st.faceTris)
        {
          // Add the verts again
          vertices.Add(tri.v1);
          normals.Add(Vector3.Normalize(origin + tri.v1));
          uvs.Add(uv0 + uvOffset);
          vertices.Add(tri.v2);
          normals.Add(Vector3.Normalize(origin + tri.v2));
          uvs.Add(uv1 + uvOffset);
          vertices.Add(tri.v3);
          normals.Add(Vector3.Normalize(origin + tri.v3));
          uvs.Add(uv1 + uvOffset);

          triangles.Add(vertices.Count - 3);
          triangles.Add(vertices.Count - 2);
          triangles.Add(vertices.Count - 1);
        }
        foreach (Triangle tri in st.sideTris)
        {
          // Add the verts again
          vertices.Add(tri.v1);
          normals.Add(Vector3.Normalize(origin + tri.v1));
          uvs.Add(uv0 + uvOffset);
          vertices.Add(tri.v2);
          normals.Add(Vector3.Normalize(origin + tri.v2));
          uvs.Add(uv1 + uvOffset);
          vertices.Add(tri.v3);
          normals.Add(Vector3.Normalize(origin + tri.v3));
          uvs.Add(uv1 + uvOffset);

          triangles.Add(vertices.Count - 3);
          triangles.Add(vertices.Count - 2);
          triangles.Add(vertices.Count - 1);
        }
      }
    }
    //GameObject centerMarker = (GameObject)GameObject.Instantiate(centerMarkerPrefab, tri.center, Quaternion.identity);
    Mesh m = new Mesh();
    m.vertices = vertices.ToArray();
    m.triangles = triangles.ToArray();
    m.normals = normals.ToArray();
    m.uv = uvs.ToArray();

    myCollider.sharedMesh = m;
    myFilter.sharedMesh = m;

    //Call our control function, which will iterate through the cyclic permutations to define 8 quadrants
    Cycle(controlx, controly, controlz);
    //Render the next quadrant
    i++;
    if (i <= 8)
    {
      RenderWorld(world, tileSet);
    }
    return output;
  }

  public void Cycle(bool x, bool y, bool z)
  {
    //8 quadrants
    if (!x && !y && !z)
    {
      controlz = true;
    }
    if (!x && !y && z)
    {
      controlz = false;
      controly = true;
    }
    if (!x && y && !z)
    {
      controlz = true;
    }
    if (!x && y && z)
    {
      controlx = true;
      controly = false;
      controlz = false;
    }
    if (x && !y && !z)
    {
      controlz = true;
    }
    if (x && !y && z)
    {
      controly = true;
      controlz = false;
    }
    if (x && y && !z)
    {
      controlz = true;
    }
    if (x && y && z)
    {
      controlx = controly = controlz = false;
    }
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
  public bool ControlX(float centerx)
  {
    if (controlx)
      return (centerx >= 0);
    else
      return (centerx <= 0);
  }
  public bool ControlY(float centery)
  {
    if (controly)
      return (centery >= 0);
    else
      return (centery <= 0);
  }
  public bool ControlZ(float centerz)
  {
    if (controlz)
      return (centerz >= 0);
    else
      return (centerz <= 0);
  }
}

