using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PolySphere
{
  public Vector3 origin;
  public int subdivisions = 3;
  public int scale = 1;

  public List<Triangle> icosahedronTris;
  public List<List<Triangle>> subdividedTris;
  public List<Triangle> finalTris;    // The finest level of subdivided tris
  public List<Hexagon> finalHexes, unitHexes;
  public List<SphereTile> sTiles;
 
  //For simplex
  public static SimplexNoise simplex;
  public float amplitude, lacunarity, persistence;
  public int octaves, multiplier;

  public PolySphere()
  {
      
  }
  public PolySphere(Vector3 o, int s, int d)
  {
    origin = o;
    scale = s;
    subdivisions = d;
    //Subdivide(d);
    //For seeding dual centers
    amplitude = Random.Range(0.01f, 0.1f);
    lacunarity = Random.Range(0.2f, 2f);
    persistence = Random.Range(0.1f, 0.9f);
    //Vector3 weights = Random.Vector3;
    octaves = Random.Range(1, 10);
    multiplier = Random.Range(1, 24);
    simplex = new SimplexNoise(GameManager.gameSeed);
     
    icosahedronTris = Icosahedron(scale);
 
    SubdivideAndDuals();

  }

  void Subdivide(int divisions)
  {
    List<Triangle> currentTris;
    List<Triangle> nextTris = new List<Triangle>(icosahedronTris);
    //List<Triforce> triforces;
    subdividedTris = new List<List<Triangle>>();

    // Subdivide icosahedron
    for (int i = 0; i < divisions; i++)
    {
      currentTris = new List<Triangle>(nextTris);
      nextTris = new List<Triangle>();
      //triforces = new List<Triforce>();

      foreach (Triangle tri in currentTris)
      {
        //Bisect
        Vector3 v1 = Vector3.Lerp(tri.v1, tri.v2, .5f);
        Vector3 v2 = Vector3.Lerp(tri.v2, tri.v3, .5f);
        Vector3 v3 = Vector3.Lerp(tri.v3, tri.v1, .5f);

        //Project onto sphere
        v1 *= (float)(1.902084 / v1.magnitude) * scale; //golden rectangle sphere radius 1.902084, as calculated by unity
        v2 *= (float)(1.902084 / v2.magnitude) * scale;
        v3 *= (float)(1.902084 / v3.magnitude) * scale;

        //Add the four new triangles
        Triangle mid = new Triangle(v1, v2, v3, tri, TriforcePosition.Mid, subdivisions);
        nextTris.Add(mid);   // Center of triforce

        Triangle top = new Triangle(tri.v1, v1, v3, tri, TriforcePosition.Top, subdivisions);
        nextTris.Add(top);

        Triangle right = new Triangle(v1, tri.v2, v2, tri, TriforcePosition.Right, subdivisions);
        nextTris.Add(right);

        Triangle left = new Triangle(v3, v2, tri.v3, tri, TriforcePosition.Left, subdivisions);
        nextTris.Add(left);

        //tri.AssignChildren(mid, top, left, right);
      }

      // --- Number tris ---
      int count = 0;
      foreach (Triangle t in nextTris)
      {
        t.index = count;
        count++;
      }
      /*
      //Set Neighbors
      foreach (Triangle tri in currentTris)
      {
        tri.childMid.AssignNeighbors(tri.childTop, tri.childRight, tri.childLeft);
        tri.childTop.AssignNeighbors(tri.NeighborOne(tri.childTop), tri.childMid, tri.NeighborTwo(tri.childTop));
        tri.childRight.AssignNeighbors(tri.NeighborOne(tri.childRight), tri.NeighborTwo(tri.childRight), tri.childMid);
        tri.childLeft.AssignNeighbors(tri.childMid, tri.NeighborOne(tri.childLeft), tri.NeighborTwo(tri.childLeft));
      }
      */

      //Save our subdivided levels
      subdividedTris.Add(nextTris);
    }

    finalTris = nextTris;

    //finalTris.RemoveAt(finalTris[0].parent.childTop.top.index);
    //finalTris.RemoveAt(finalTris[0].parent.childRight.top.index);
    //finalTris.RemoveAt(finalTris[0].parent.childLeft.top.index);
  }


  // BARRIER
  // OF
  // USEFULNESS
  // BETWEEN
  // TWO
  // SIMILAR
  // FUNCTIONS


  void SubdivideAndDuals()
  {
    List<Triangle> currentTris;
    List<Triangle> nextTris = new List<Triangle>(icosahedronTris); //Original icosahedron
    List<List<Triangle>> subdividedTris = new List<List<Triangle>>();

    sTiles = new List<SphereTile>();
    
    // Subdivide icosahedron
    for (int i = 0; i < subdivisions; i++)
    {
      currentTris = new List<Triangle>(nextTris);
      nextTris = new List<Triangle>();
      //triforces = new List<Triforce>();

      foreach (Triangle tri in currentTris)
      {
        //Bisect
        Vector3 v1 = (tri.v1+tri.v2)/2.0f;
        Vector3 v2 = (tri.v2+tri.v3)/2.0f;
        Vector3 v3 = (tri.v3+tri.v1)/2.0f;

        //Project onto sphere
        v1 *= (float)(1.902084 / v1.magnitude) * scale; //golden rectangle sphere radius 1.902084
        v2 *= (float)(1.902084 / v2.magnitude) * scale;
        v3 *= (float)(1.902084 / v3.magnitude) * scale;

        //Add the four new triangles
        Triangle mid = new Triangle(v1, v2, v3, tri, TriforcePosition.Mid, subdivisions);
        nextTris.Add(mid);   // Center of triforce

        Triangle top = new Triangle(tri.v1, v1, v3, tri, TriforcePosition.Top, subdivisions);
        nextTris.Add(top);

        Triangle right = new Triangle(v1, tri.v2, v2, tri, TriforcePosition.Right, subdivisions);
        nextTris.Add(right);

        Triangle left = new Triangle(v3, v2, tri.v3, tri, TriforcePosition.Left, subdivisions);
        nextTris.Add(left);

        //tri.AssignChildren(mid, top, left, right);
      }
      /*
      //Set Neighbors
      foreach (Triangle tri in currentTris)
      {

        tri.childMid.AssignNeighbors(tri.childTop, tri.childRight, tri.childLeft);
        tri.childTop.AssignNeighbors(tri.NeighborOne(tri.childTop), tri.childMid, tri.NeighborTwo(tri.childTop));
        tri.childRight.AssignNeighbors(tri.NeighborOne(tri.childRight), tri.NeighborTwo(tri.childRight), tri.childMid);
        tri.childLeft.AssignNeighbors(tri.childMid, tri.NeighborOne(tri.childLeft), tri.NeighborTwo(tri.childLeft));

      }
      */
      //Save our subdivided levels
      subdividedTris.Add(nextTris); 
    }
    //Now, after subdivision, create the dual
    //Debug.Log(((nextTris[0].center + nextTris[12].center) / 2).magnitude);
    //Debug.Log(((nextTris[3].center + nextTris[14].center) / 2).magnitude);
    //Debug.Log(((nextTris[2].center + nextTris[13].center) / 2).magnitude);
    
    //Create SphereTiles, give them neighbors
    
    foreach (Triangle tri in nextTris)
    {
      //Tiles to assign
      SphereTile st1 = null, 
                 st2 = null, 
                 st3 = null;
      
      //Create empty SphereTiles, or, if we've already created a SphereTile at this point just reference it
      foreach (SphereTile st in sTiles)
      {
        //Debug.Log((st.center - tri.v1).sqrMagnitude);
        //Debug.Log((st.center - tri.v2).sqrMagnitude);
        //Debug.Log((st.center - tri.v3).sqrMagnitude);
        if ((Vector3)st.center == (Vector3)tri.v1)
        {
          st1 = st;
        }
        if ((Vector3)st.center == (Vector3)tri.v2)
        {
          st2 = st;
        }
        if ((Vector3)st.center == (Vector3)tri.v3)
        {
          st3 = st;
        }
      }
      if (st1 == null)
      {
        st1 = new SphereTile(tri.v1);
        sTiles.Add(st1);
      }
      if (st2 == null)
      {
        st2 = new SphereTile(tri.v2);
        sTiles.Add(st2);
      }
      if (st3 == null)
      {
        st3 = new SphereTile(tri.v3);
        sTiles.Add(st3);
      }

      /*
      //Add in the new neighbors from this triangle
      st1.neighbors.Add(st2);
      st1.neighbors.Add(st3);

      st2.neighbors.Add(st1);
      st2.neighbors.Add(st3);

      st3.neighbors.Add(st1);
      st3.neighbors.Add(st2);
      */

      //Add this triangle as an inital triangle in each spheretile
      st1.subTriangles.Add(tri);
      st2.subTriangles.Add(tri);
      st3.subTriangles.Add(tri);
    }
    //dualCenters = dualCenters.Distinct().ToList();
    
    //Build the SphereTiles!
    foreach(SphereTile st in sTiles)
    {
      st.Build();
      st.scale *= scale;
    }
    
    // === Cache unit hexagons ===
    unitHexes = new List<Hexagon>();
    foreach (SphereTile st in sTiles)
    {
      unitHexes.Add(st.ToHexagon());
    }

    //******* Then, we can run scale functions on it to make individual worlds
    //Seed
    //ScaleTrigPerlin();
    ScaleSimplex(PolySphere.simplex, octaves, multiplier, amplitude, lacunarity, persistence);

    //Set water depth using average of all scales 
    float sAverage = 0;
    foreach (SphereTile st in sTiles)
    {
      sAverage += st.scale;
    }
    sAverage /= sTiles.Count;
   
   // === Cache final hexes ===
   finalHexes = new List<Hexagon>();
    foreach (SphereTile st in sTiles)
    {
      finalHexes.Add(st.ToHexagon());
    }

    // --- Number tris ---
    int count = 0;
    foreach (Triangle t in nextTris)
    {
      t.index = count;
      count++;
    }
    //finalTris = dualTris;
  }
  /*  // Haven't yet been converted to work in PolySphere.cs
  public void ScaleTrigPerlin() //@TODO: think about this more
  {
    //float seed = Random.Range(0.1f,0.9f);
    //float perlin = Mathf.PerlinNoise((center.x / 10.0f + seedx) * lacunarity, (center.y / 10.0f + seedy) * lacunarity);
    //scale *= (Mathf.Pow(seedy*Mathf.Sin((center.x + perlin)*seedx), 2) + Mathf.Pow(seedy*Mathf.Sin((center.y + perlin)*seedx), 2) + seedy*Mathf.Pow(Mathf.Sin((center.z+perlin)*seedx), 2));
    //math.sin(0.2 + (i * 0.08) * math.cos(0.4 + i * 0.3)
    foreach (SphereTile st in sTiles)
    {
      st.scale *= 1 + (int)(Mathf.Abs((weightx * Mathf.Sin(randx * st.center.x) + weighty * Mathf.Sin(randy * st.center.y) + weightz * Mathf.Sin(randz * st.center.z)
                            + weightx * Mathf.Cos(randx * st.center.x) + weighty * Mathf.Cos(randy * st.center.y) + weightz * Mathf.Cos(randz * st.center.z)))*100) / 100f;
    }
  }
  
  
  public void ScalePerlin(int octaves, int multiplier, float amplitude, int lacunarity, float persistence)
  {
    foreach(SphereTile st in sTiles)
    {
      float height = Mathf.PerlinNoise((st.center.x / 10.0f + randx) * lacunarity, (st.center.y / 10.0f + randy) * lacunarity);
      st.scale *= height;
    }
  }
  */

  public void ScaleSimplex(SimplexNoise simplex, int octaves, int multiplier, float amplitude, float lacunarity, float persistence)
  {
    foreach (SphereTile st in sTiles)
    {
      float height = Mathf.Abs(simplex.coherentNoise(st.center.x, st.center.y, st.center.z, octaves, multiplier, amplitude, lacunarity, persistence));
      st.scale *= 24*(1 + height*100);
      Debug.Log(height);
    }
  }
  
  List<Triangle> Icosahedron(int scale)
  {
    List<Triangle> output = new List<Triangle>();
    List<Vector3> vertices = new List<Vector3>();

    float goldRat = 1.6f; //golden ratio (Unity calculated it as 1.6)

    //Icosahedron coords
    Vector3 origin = Vector3.zero,
            xy1 = new Vector3(1, goldRat, 0) * scale,
            xy2 = new Vector3(1, -goldRat, 0) * scale,
            xy3 = new Vector3(-1, -goldRat, 0) * scale,
            xy4 = new Vector3(-1, goldRat, 0) * scale,
            xz1 = new Vector3(goldRat, 0, 1) * scale,
            xz2 = new Vector3(goldRat, 0, -1) * scale,
            xz3 = new Vector3(-goldRat, 0, -1) * scale,
            xz4 = new Vector3(-goldRat, 0, 1) * scale,
            zy1 = new Vector3(0, 1, goldRat) * scale,
            zy2 = new Vector3(0, 1, -goldRat) * scale,
            zy3 = new Vector3(0, -1, -goldRat) * scale,
            zy4 = new Vector3(0, -1, goldRat) * scale;
    //Debug.Log(xz4.magnitude);
    vertices.Add(origin);         // 0
    vertices.Add(origin + xy1);   // 1
    vertices.Add(origin + xy2);   // 2
    vertices.Add(origin + xy3);   // 3
    vertices.Add(origin + xy4);   // 4
    vertices.Add(origin + xz1);   // 5
    vertices.Add(origin + xz2);   // 6
    vertices.Add(origin + xz3);   // 7
    vertices.Add(origin + xz4);   // 8
    vertices.Add(origin + zy1);   // 9
    vertices.Add(origin + zy2);   // 10
    vertices.Add(origin + zy3);   // 11
    vertices.Add(origin + zy4);   // 12

    // === Faces of the Original 5 Triforces ===
    output.Add(new Triangle(vertices[1], vertices[6], vertices[10]));   // 0
    output.Add(new Triangle(vertices[1], vertices[10], vertices[4]));   // 1
    output.Add(new Triangle(vertices[1], vertices[4], vertices[9]));    // 2
    output.Add(new Triangle(vertices[1], vertices[9], vertices[5]));    // 3
    output.Add(new Triangle(vertices[1], vertices[5], vertices[6]));    // 4
    
    output.Add(new Triangle(vertices[3], vertices[7], vertices[11]));   // 7
    output.Add(new Triangle(vertices[3], vertices[11], vertices[2]));   // 6
    output.Add(new Triangle(vertices[3], vertices[2], vertices[12]));   // 5
    output.Add(new Triangle(vertices[3], vertices[12], vertices[8]));   // 9
    output.Add(new Triangle(vertices[3], vertices[8], vertices[7]));    // 8

    output.Add(new Triangle(vertices[10], vertices[7], vertices[4]));   // 10
    output.Add(new Triangle(vertices[4], vertices[7], vertices[8]));    // 11
    output.Add(new Triangle(vertices[4], vertices[8], vertices[9]));    // 12
    output.Add(new Triangle(vertices[9], vertices[8], vertices[12]));   // 13
    output.Add(new Triangle(vertices[9], vertices[12], vertices[5]));   // 14
    output.Add(new Triangle(vertices[5], vertices[12], vertices[2]));   // 15
    output.Add(new Triangle(vertices[5], vertices[2], vertices[6]));    // 16
    output.Add(new Triangle(vertices[6], vertices[2], vertices[11]));   // 17
    output.Add(new Triangle(vertices[11], vertices[10], vertices[6]));  // 18
    output.Add(new Triangle(vertices[10], vertices[11], vertices[7]));  // 19

    /*
    // Assign initial neighbors
    output[0].AssignNeighbors(output[1], output[4], output[18]);
    output[1].AssignNeighbors(output[2], output[0], output[10]);
    output[2].AssignNeighbors(output[1], output[12],output[3]);
    output[3].AssignNeighbors(output[2], output[14],output[4]);
    output[4].AssignNeighbors(output[3], output[16],output[0]);
    output[5].AssignNeighbors(output[19],output[6],output[9]);
    output[6].AssignNeighbors(output[5], output[17],output[7]);
    output[7].AssignNeighbors(output[6], output[15],output[8]);
    output[8].AssignNeighbors(output[7], output[13],output[9]);
    output[9].AssignNeighbors(output[8], output[11],output[5]);
    output[10].AssignNeighbors(output[1], output[19],output[11]);
    output[11].AssignNeighbors(output[10],output[9], output[12]);
    output[12].AssignNeighbors(output[11],output[13],output[2]);
    output[13].AssignNeighbors(output[12],output[8], output[14]);
    output[14].AssignNeighbors(output[13],output[15],output[3]);
    output[15].AssignNeighbors(output[14],output[7], output[16]);
    output[16].AssignNeighbors(output[15],output[17],output[4]);
    output[17].AssignNeighbors(output[16],output[6], output[18]);
    output[18].AssignNeighbors(output[17],output[19],output[0]);
    output[19].AssignNeighbors(output[18],output[5], output[10]);
    */

    // --- Number tris ---
    int count = 0;
    foreach (Triangle t in output)
    {
      t.index = count;
      count++;
    }

    return output;
  }
}
