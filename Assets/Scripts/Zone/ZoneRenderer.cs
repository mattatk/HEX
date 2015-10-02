using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoneRenderer : MonoBehaviour
{
  public GameObject boardPrefab;

	public GameObject RenderZone(Zone zone, TileSet tileSet)
  {
    Vector3 zonePlacement = Vector3.zero;//new Vector3(-1*Hex.bisect*zone.width, 0, Hex.sideAndAHalf*zone.width/-2);
    GameObject output = (GameObject)Instantiate(boardPrefab, zonePlacement, Quaternion.identity);

    MeshFilter myFilter = output.GetComponent<MeshFilter>();
    MeshCollider myCollider = output.GetComponent<MeshCollider>();

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector3> normals = new List<Vector3>();
    List<Vector2>uvs = new List<Vector2>();

    
    Vector3 origin,
                v1 = new Vector3(0,0,Hex.edge),
                v2 = new Vector3(Hex.bisect, 0, Hex.halfSide),
                v3 = new Vector3(Hex.bisect, 0, -Hex.halfSide),
                v4 = new Vector3(0,0,-Hex.edge),
                v5 = new Vector3(-Hex.bisect,0,-Hex.halfSide),
                v6 = new Vector3(-Hex.bisect, 0, Hex.halfSide);

    float texHeight = tileSet.texture.height;
    float texWidth = tileSet.texture.width;
    float root3 = Mathf.Sqrt(3);
    float uvTileWidth = tileSet.tileWidth/texWidth;
    float uvTileHeight = tileSet.tileWidth/texHeight;
    float side = uvTileWidth/2;
    float radius = Mathf.Sqrt((3*side*side)/4);
    float cacheA = Mathf.Sqrt(Mathf.Pow(side,2)-Mathf.Pow(side,2));

    Vector2 uv0 = new Vector2(side,side),
            uv1 = new Vector2(side, side+side),
            uv2 = new Vector2(side+radius, side+side/2),
            uv3 = new Vector2(side+radius, side/2),
            uv4 = new Vector2(side, 0),
            uv5 = new Vector2(side-radius, side/2),
            uv6 = new Vector2(side-radius, side+side/2);

    int counter = 0;
    for (int x=0; x<zone.width; x++)
    {
      for (int y=0;y<zone.width; y++)
      {
        TileType tileType = zone.tiles[x,y].type;

        if (tileType == TileType.None)
          continue;
        /*
                1 = 0,side
            6         2=height, side/2
            origin(0)
            5         3=height, -side/2
                4= 0,-side
        */

        IntCoord uvCoord = tileSet.GetUVForType(tileType);
        //Debug.Log(uvCoord.ToVector2());
        //Debug.Log(uvCoord.ToVector2() * uvTileWidth);
        Vector2 uvOffset = new Vector2(uvCoord.x*uvTileWidth, uvCoord.y*uvTileHeight);
        //Vector2 uvOffset = new Vector2(256/texWidth,256/texWidth);
        //Vector2 uvOffset = Vector2.zero;

        float xOffset = x*Hex.doubleHeight;
        Vector3 tileHeight = Vector3.up*zone.tiles[x,y].height;

        if (y%2==1)
          xOffset += Hex.bisect;

        origin = new Vector3(xOffset, 0, y*Hex.sideAndAHalf);

        // Add the first hexagon. Vertex 0
        vertices.Add(origin);
        normals.Add(Vector3.up);
        uvs.Add(uv0+uvOffset);
    //1
        vertices.Add(origin+v1);
        normals.Add(Vector3.up);
        uvs.Add(uv1+uvOffset);
    //2
        vertices.Add(origin+v2);
        normals.Add(Vector3.up);
        uvs.Add(uv2+uvOffset);
    //3
        vertices.Add(origin+v3);
        normals.Add(Vector3.up);
        uvs.Add(uv3+uvOffset);
    //4
        vertices.Add(origin+v4);
        normals.Add(Vector3.up);
        uvs.Add(uv4+uvOffset);
    //5
        vertices.Add(origin+v5);
        normals.Add(Vector3.up);
        uvs.Add(uv5+uvOffset);
    //6
        vertices.Add(origin+v6);
        normals.Add(Vector3.up);
        uvs.Add(uv6+uvOffset);
    
    //Second hex for depth.  Vertex 7
    vertices.Add (origin + tileHeight);
    normals.Add (Vector3.up);
    uvs.Add (uv0+uvOffset);
    //8
    vertices.Add (origin+v1+ tileHeight);
    normals.Add (Vector3.up);
    uvs.Add (uv1+uvOffset);
    //9
    vertices.Add (origin+v2+ tileHeight);
    normals.Add (Vector3.up);
    uvs.Add (uv2+uvOffset);
    //10
    vertices.Add (origin+v3+ tileHeight);
    normals.Add (Vector3.up);
    uvs.Add (uv3+uvOffset);
    //11
    vertices.Add (origin+v4+ tileHeight);
    normals.Add (Vector3.up);
    uvs.Add (uv4+uvOffset);
    //12
    vertices.Add (origin+v5+ tileHeight);
    normals.Add (Vector3.up);
    uvs.Add (uv5+uvOffset);
    //13
    vertices.Add (origin+v6+ tileHeight);
    normals.Add (Vector3.up);
    uvs.Add (uv6+uvOffset);

        /*
              .....
             / 6|  /\
            / \ |1/  \
           / 5 \|/  2 \
           \  /4|3\  /
            \/..|..\/
        */
    /*
        //We don't need the bottom face anymore
        // Triangle 1
        triangles.Add(counter);
        triangles.Add(counter+1);
        triangles.Add(counter+2);
        // Triangle 2
        triangles.Add(counter);
        triangles.Add(counter+2);
        triangles.Add(counter+3);
        // Triangle 3
        triangles.Add(counter);
        triangles.Add(counter+3);
        triangles.Add(counter+4);
        // Triangle 4
        triangles.Add(counter);
        triangles.Add(counter+4);
        triangles.Add(counter+5);
        // Triangle 5
        triangles.Add(counter);
        triangles.Add(counter+5);
        triangles.Add(counter+6);
        // Triangle 6
        triangles.Add(counter);
        triangles.Add(counter+6);
        triangles.Add(counter+1);
    */

    //Height Triangle 7
    triangles.Add (counter + 7);
    triangles.Add (counter + 13);
    triangles.Add (counter + 8);
    //8
    triangles.Add (counter + 7);
    triangles.Add (counter + 8);
    triangles.Add (counter + 9);
    //9
    triangles.Add (counter + 7);
    triangles.Add (counter + 9);
    triangles.Add (counter + 10);
    //10
    triangles.Add (counter + 7);
    triangles.Add (counter + 10);
    triangles.Add (counter + 11);
    //11
    triangles.Add (counter + 7);
    triangles.Add (counter + 11);
    triangles.Add (counter + 12);
    //12
    triangles.Add (counter + 7);
    triangles.Add (counter + 12);
    triangles.Add (counter + 13);


    //The 6 Parallelograms (12 triangles) which complete the hexagon

    triangles.Add(counter + 1);
    triangles.Add(counter + 8);
    triangles.Add(counter + 13);
    triangles.Add (counter + 1);
    triangles.Add (counter + 13);
    triangles.Add (counter + 6);
    
    triangles.Add (counter + 2);
    triangles.Add (counter + 8);
    triangles.Add (counter + 1);
    triangles.Add (counter + 2);
    triangles.Add (counter + 9);
    triangles.Add (counter + 8);

    triangles.Add (counter + 3);
    triangles.Add (counter + 9);
    triangles.Add (counter + 2);
    triangles.Add (counter + 3);
    triangles.Add (counter + 10);
    triangles.Add (counter + 9);

    triangles.Add (counter + 4);
    triangles.Add (counter + 10);
    triangles.Add (counter + 3);
    triangles.Add (counter + 4);
    triangles.Add (counter + 11);
    triangles.Add (counter + 10);

    triangles.Add (counter + 5);
    triangles.Add (counter + 11);
    triangles.Add (counter + 4);
    triangles.Add (counter + 5);
    triangles.Add (counter + 12);
    triangles.Add (counter + 11);

    triangles.Add (counter + 6);
    triangles.Add (counter + 12);
    triangles.Add (counter + 5);
    triangles.Add (counter + 6);
    triangles.Add (counter + 13);
    triangles.Add (counter + 12);
    
    counter += 14;
    }
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
