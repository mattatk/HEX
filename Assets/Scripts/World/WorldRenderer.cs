using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldRenderer : MonoBehaviour
{
  public GameObject worldPrefab;

  public GameObject RenderWorld(World world, TileSet tileSet)
  {
    
    GameObject output = (GameObject)Instantiate(worldPrefab, Vector3.zero, Quaternion.identity);

    MeshFilter myFilter = output.GetComponent<MeshFilter>();
    MeshCollider myCollider = output.GetComponent<MeshCollider>();

    List<Triangle> allTriangles = new List<Triangle>();
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector3> normals = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();

    float goldRat = (1 + Mathf.Sqrt(5)) / 2;

    Vector2 uv0 = new Vector2(0, 0),
            uv1 = new Vector2(.5f, 1),
            uv2 = new Vector2(.5f, 1);
      
    Vector2 uvOffset = Vector3.zero;

    //Icosahedron coords
    Vector3 origin = Vector3.zero,
            xy1 = new Vector3(1, goldRat, 0),
            xy2 = new Vector3(1, -goldRat, 0),
            xy3 = new Vector3(-1, -goldRat, 0),
            xy4 = new Vector3(-1, goldRat, 0),
            xz1 = new Vector3(goldRat, 0, 1),
            xz2 = new Vector3(goldRat, 0, -1),
            xz3 = new Vector3(-goldRat, 0, -1),
            xz4 = new Vector3(-goldRat, 0, 1),
            zy1 = new Vector3(0, 1, goldRat),
            zy2 = new Vector3(0, 1, -goldRat),
            zy3 = new Vector3(0, -1, -goldRat),
            zy4 = new Vector3(0, -1, goldRat);

    //0
    vertices.Add(origin);
    normals.Add(Vector3.up);
    uvs.Add(uv0 + uvOffset);
    //1
    vertices.Add(origin + xy1);
    normals.Add(Vector3.Normalize(origin + xy1));
    uvs.Add(uv0 + uvOffset);
    //2
    vertices.Add(origin + xy2);
    normals.Add(Vector3.Normalize(origin + xy2));
    uvs.Add(uv2 + uvOffset);
    //3
    vertices.Add(origin + xy3);
    normals.Add(Vector3.Normalize(origin + xy3));
    uvs.Add(uv0 + uvOffset);
    //4
    vertices.Add(origin + xy4);
    normals.Add(Vector3.Normalize(origin + xy4));
    uvs.Add(uv1 + uvOffset);
    //5
    vertices.Add(origin + xz1);
    normals.Add(Vector3.Normalize(origin + xz1));
    uvs.Add(uv1 + uvOffset);
    //6
    vertices.Add(origin + xz2);
    normals.Add(Vector3.Normalize(origin + xz2));
    uvs.Add(uv1 + uvOffset);
    //7
    vertices.Add(origin + xz3);
    normals.Add(Vector3.Normalize(origin + xz3));
    uvs.Add(uv2 + uvOffset);
    //8
    vertices.Add(origin + xz4);
    normals.Add(Vector3.Normalize(origin + xz4));
    uvs.Add(uv2 + uvOffset);
    //9
    vertices.Add(origin + zy1);
    normals.Add(Vector3.Normalize(origin + zy1));
    uvs.Add(uv1 + uvOffset);
    //10
    vertices.Add(origin + zy2);
    normals.Add(Vector3.Normalize(origin + zy2));
    uvs.Add(uv1 + uvOffset);
    //11
    vertices.Add(origin + zy3);
    normals.Add(Vector3.Normalize(origin + zy3));
    uvs.Add(uv2 + uvOffset);
    //12
    vertices.Add(origin + zy4);
    normals.Add(Vector3.Normalize(origin + zy4));
    uvs.Add(uv1 + uvOffset);

    int counter = 0;
    // Triangle 1
    allTriangles.Add(new Triangle(vertices[1], vertices[6], vertices[10]));
    triangles.Add(counter + 1);
    triangles.Add(counter + 6);
    triangles.Add(counter + 10);
    // Triangle 2
    allTriangles.Add(new Triangle(vertices[1], vertices[10], vertices[4]));
    triangles.Add(counter + 1);
    triangles.Add(counter + 10);
    triangles.Add(counter + 4);
    // Triangle 3
    allTriangles.Add(new Triangle(vertices[1], vertices[4], vertices[9]));
    triangles.Add(counter + 1);
    triangles.Add(counter + 4);
    triangles.Add(counter + 9);
    // Triangle 4
    allTriangles.Add(new Triangle(vertices[1], vertices[9], vertices[5]));
    triangles.Add(counter + 1);
    triangles.Add(counter + 9);
    triangles.Add(counter + 5);
    // Triangle 5
    allTriangles.Add(new Triangle(vertices[1], vertices[5], vertices[6]));
    triangles.Add(counter + 1);
    triangles.Add(counter + 5);
    triangles.Add(counter + 6);

    // Triangle 6
    allTriangles.Add(new Triangle(vertices[3], vertices[7], vertices[11]));
    triangles.Add(counter + 3);
    triangles.Add(counter + 7);
    triangles.Add(counter + 11);
    // Triangle 7
    allTriangles.Add(new Triangle(vertices[3], vertices[11], vertices[2]));
    triangles.Add(counter + 3);
    triangles.Add(counter + 11);
    triangles.Add(counter + 2);
    // Triangle 8
    allTriangles.Add(new Triangle(vertices[3], vertices[2], vertices[12]));
    triangles.Add(counter + 3);
    triangles.Add(counter + 2);
    triangles.Add(counter + 12);
    // Triangle 9'
    allTriangles.Add(new Triangle(vertices[3], vertices[12], vertices[8]));
    triangles.Add(counter + 3);
    triangles.Add(counter + 12);
    triangles.Add(counter + 8);
    // Triangle 10
    allTriangles.Add(new Triangle(vertices[3], vertices[8], vertices[7]));
    triangles.Add(counter + 3);
    triangles.Add(counter + 8);
    triangles.Add(counter + 7);

    //Triangle 11
    allTriangles.Add(new Triangle(vertices[10], vertices[7], vertices[4]));
    triangles.Add(counter + 10);
    triangles.Add(counter + 7);
    triangles.Add(counter + 4);
    // Triangle 12
    allTriangles.Add(new Triangle(vertices[4], vertices[7], vertices[8]));
    triangles.Add(counter + 4);
    triangles.Add(counter + 7);
    triangles.Add(counter + 8);
    // Triangle 13
    allTriangles.Add(new Triangle(vertices[4], vertices[8], vertices[9]));
    triangles.Add(counter + 4);
    triangles.Add(counter + 8);
    triangles.Add(counter + 9);
    // Triangle 14
    allTriangles.Add(new Triangle(vertices[9], vertices[8], vertices[12]));
    triangles.Add(counter + 9);
    triangles.Add(counter + 8);
    triangles.Add(counter + 12);
    // Triangle 15
    allTriangles.Add(new Triangle(vertices[9], vertices[12], vertices[5]));
    triangles.Add(counter + 9);
    triangles.Add(counter + 12);
    triangles.Add(counter + 5);
    // Triangle 16
    allTriangles.Add(new Triangle(vertices[5], vertices[12], vertices[2]));
    triangles.Add(counter + 5);
    triangles.Add(counter + 12);
    triangles.Add(counter + 2);
    // Triangle 17
    allTriangles.Add(new Triangle(vertices[5], vertices[2], vertices[6]));
    triangles.Add(counter + 5);
    triangles.Add(counter + 2);
    triangles.Add(counter + 6);
    // Triangle 18
    allTriangles.Add(new Triangle(vertices[6], vertices[2], vertices[11]));
    triangles.Add(counter + 6);
    triangles.Add(counter + 2);
    triangles.Add(counter + 11);
    // Triangle 19
    allTriangles.Add(new Triangle(vertices[6], vertices[11], vertices[10]));
    triangles.Add(counter + 6);
    triangles.Add(counter + 11);
    triangles.Add(counter + 10);
    // Triangle 20
    allTriangles.Add(new Triangle(vertices[10], vertices[11], vertices[7]));
    triangles.Add(counter + 10);
    triangles.Add(counter + 11);
    triangles.Add(counter + 7);

    //Subdivide
    List<Triangle> triCopy = new List<Triangle>();
    foreach (Triangle tri in allTriangles)
    {
      triCopy.Add(tri);
    }

    foreach (Triangle tri in allTriangles)
    {
      Debug.Log(tri.v3);
      Vector3 v1 = (tri.v1 - tri.v3)*(1/2);
      Vector3 v2 = (tri.v2 - tri.v1)*(1/2);
      Vector3 v3 = (tri.v3 - tri.v2)*(1/2);

      //Project onto sphere
      v1 *= (float)(1.9 / v1.magnitude); //golden rectangle sphere radius 1.902
      v2 *= (float)(1.9 / v2.magnitude);
      v3 *= (float)(1.9 / v3.magnitude);

      triCopy.Add(new Triangle(v1, v2, v3));

      vertices.Add(v1);
      normals.Add(Vector3.Normalize(origin + v1));
      uvs.Add(uv1 + uvOffset);

      vertices.Add(v2);
      normals.Add(Vector3.Normalize(origin + v2));
      uvs.Add(uv1 + uvOffset);

      vertices.Add(v3);
      normals.Add(Vector3.Normalize(origin + v3));
      uvs.Add(uv1 + uvOffset);

      triangles.Add(vertices.Count - 3);
      triangles.Add(vertices.Count - 2);
      triangles.Add(vertices.Count-1);
    }

    allTriangles.Clear();
    foreach(Triangle tri in triCopy)
    {
      allTriangles.Add(tri);
    }

    Mesh m = new Mesh();
    m.vertices = vertices.ToArray();
    m.triangles = triangles.ToArray();
    m.normals = normals.ToArray();
    m.uv = uvs.ToArray();

    myCollider.sharedMesh = m;
    myFilter.sharedMesh = m;

    return output;
    }
}

