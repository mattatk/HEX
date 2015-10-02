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

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();

        float goldRat = (1 + Mathf.Sqrt(5)) / 2;

        Vector2 uv0 = new Vector2(0, 0),
                uv1 = new Vector2(.5f, 1),
                uv2 = new Vector2(1, 0);
      
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
        triangles.Add(counter + 1);
        triangles.Add(counter + 6);
        triangles.Add(counter + 10);
        // Triangle 2
        triangles.Add(counter + 1);
        triangles.Add(counter + 10);
        triangles.Add(counter + 4);
        // Triangle 3
        triangles.Add(counter + 1);
        triangles.Add(counter + 4);
        triangles.Add(counter + 9);
        // Triangle 4
        triangles.Add(counter + 1);
        triangles.Add(counter + 9);
        triangles.Add(counter + 5);
        // Triangle 5
        triangles.Add(counter + 1);
        triangles.Add(counter + 5);
        triangles.Add(counter + 6);

        // Triangle 6
        triangles.Add(counter + 3);
        triangles.Add(counter + 7);
        triangles.Add(counter + 11);
        // Triangle 7
        triangles.Add(counter + 3);
        triangles.Add(counter + 11);
        triangles.Add(counter + 2);
        // Triangle 8
        triangles.Add(counter + 3);
        triangles.Add(counter + 2);
        triangles.Add(counter + 12);
        // Triangle 9
        triangles.Add(counter + 3);
        triangles.Add(counter + 12);
        triangles.Add(counter + 8);
        // Triangle 10
        triangles.Add(counter + 3);
        triangles.Add(counter + 8);
        triangles.Add(counter + 7);

        //Triangle 11
        triangles.Add(counter + 10);
        triangles.Add(counter + 7);
        triangles.Add(counter + 4);
        // Triangle 12
        triangles.Add(counter + 4);
        triangles.Add(counter + 7);
        triangles.Add(counter + 8);
        // Triangle 13
        triangles.Add(counter + 4);
        triangles.Add(counter + 8);
        triangles.Add(counter + 9);
        // Triangle 14
        triangles.Add(counter + 9);
        triangles.Add(counter + 8);
        triangles.Add(counter + 12);
        // Triangle 15
        triangles.Add(counter + 9);
        triangles.Add(counter + 12);
        triangles.Add(counter + 5);
        // Triangle 16
        triangles.Add(counter + 5);
        triangles.Add(counter + 12);
        triangles.Add(counter + 2);
        // Triangle 17
        triangles.Add(counter + 5);
        triangles.Add(counter + 2);
        triangles.Add(counter + 6);
        // Triangle 18
        triangles.Add(counter + 6);
        triangles.Add(counter + 2);
        triangles.Add(counter + 11);
        // Triangle 19
        triangles.Add(counter + 6);
        triangles.Add(counter + 11);
        triangles.Add(counter + 10);
        // Triangle 20
        triangles.Add(counter + 10);
        triangles.Add(counter + 11);
        triangles.Add(counter + 7);

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

