using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PolySphere
{
  public List<Triangle> icosahedronTris;
  public List<List<Triangle>> subdividedTris;
  public List<Triangle> finalTris;    // The finest level of subdivided tris
  //public List<Triforce> triforces;
  int scale, subdivisions;

  public PolySphere(int s, int d)
  {
    scale = s;
    subdivisions = d;
    icosahedronTris = Icosahedron(scale);
    //Subdivide(d);
    SubdivideAndDuals(d);
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

        tri.AssignChildren(mid, top, left, right);
      }

      // --- Number tris ---
      int count = 0;
      foreach (Triangle t in nextTris)
      {
        t.index = count;
        count++;
      }
      //Set Neighbors
      foreach (Triangle tri in currentTris)
      {
        tri.childMid.AssignNeighbors(tri.childTop, tri.childRight, tri.childLeft);
        tri.childTop.AssignNeighbors(tri.NeighborOne(tri.childTop), tri.childMid, tri.NeighborTwo(tri.childTop));
        tri.childRight.AssignNeighbors(tri.NeighborOne(tri.childRight), tri.NeighborTwo(tri.childRight), tri.childMid);
        tri.childLeft.AssignNeighbors(tri.childMid, tri.NeighborOne(tri.childLeft), tri.NeighborTwo(tri.childLeft));
      }

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


  void SubdivideAndDuals(int divisions)
  {
    List<Triangle> currentTris;
    List<Triangle> nextTris = new List<Triangle>(icosahedronTris); //Original icosahedron
    List<List<Triangle>> subdividedTris = new List<List<Triangle>>();
    List<Triangle> dualTris = new List<Triangle>();
    List<SphereTile> sTiles = new List<SphereTile>();

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

        tri.AssignChildren(mid, top, left, right);
      }
      //Set Neighbors
      foreach (Triangle tri in currentTris)
      {

        tri.childMid.AssignNeighbors(tri.childTop, tri.childRight, tri.childLeft);
        tri.childTop.AssignNeighbors(tri.NeighborOne(tri.childTop), tri.childMid, tri.NeighborTwo(tri.childTop));
        tri.childRight.AssignNeighbors(tri.NeighborOne(tri.childRight), tri.NeighborTwo(tri.childRight), tri.childMid);
        tri.childLeft.AssignNeighbors(tri.childMid, tri.NeighborOne(tri.childLeft), tri.NeighborTwo(tri.childLeft));

      }
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
      //Randomize the center heights
      
      //These are the centers of the hexagons
      tri.v1 *= (float)(1.74 / tri.v1.magnitude) * scale; // 1.74 magnitude roughly on the hexagon plane, this needs improvement 
                                                          //Unity calculates further digits different for each hex, according to my debug
      tri.v2 *= (float)(1.74 / tri.v2.magnitude) * scale;
      tri.v3 *= (float)(1.74 / tri.v3.magnitude) * scale;
      //Create empty SphereTiles, or, if we've already created a SphereTile at this point just reference it
      foreach (SphereTile st in sTiles)
      {
        //Debug.Log((st.center - tri.v1).sqrMagnitude);
        //Debug.Log((st.center - tri.v2).sqrMagnitude);
        //Debug.Log((st.center - tri.v3).sqrMagnitude);
        if (st.center == tri.v1)
        {
          st1 = st;
        }
        if ((st.center == tri.v2))
        {
          st2 = st;
        }
        if ((st.center == tri.v3))
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
      //Add in the new neighbors from this triangle
      st1.neighbors.Add(st2);
      st1.neighbors.Add(st3);

      st2.neighbors.Add(st1);
      st2.neighbors.Add(st3);

      st3.neighbors.Add(st1);
      st3.neighbors.Add(st2);

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
      foreach (Triangle t in st.triangles)
      {
        dualTris.Add(t);
      }
    }
    
    // --- Number tris ---
    int count = 0;
    foreach (Triangle t in nextTris)
    {
      t.index = count;
      count++;
    }
    finalTris = dualTris;
  }

  List<Triangle> Icosahedron(int scale)
  {
    List<Triangle> output = new List<Triangle>();
    List<Vector3> vertices = new List<Vector3>();

    float goldRat = 1.618f; //golden ratio

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
/*
public class Vertex
{
  public Vector3 vertex;
  public bool builtHere = false;

  public Vertex(Vector3 v)
  {
    vertex = v;
    builtHere = false;
  }
  public Vertex(Vector3 v, bool built)
  {
    vertex = v;
    builtHere = built;
  }
}
*/
//Each SphereTile is a piece of the dual polysphere,
//all together they are the dual polysphere
public class SphereTile
{
  //The inital triangles from the subdivided polysphere which we will use to build the spheretile
  public List<Triangle> subTriangles;
  //The triangles that make up this piece of the entire dual polygon
  public List<Triangle> triangles;
  //Neighbor dual faces
  public List<SphereTile> neighbors;
  //Vertices and side vector
  public Vector3 v1, v2, v3, v4, v5, v6, side;
  //Checking equality with the center vertex (was going to anyway)
  public Vector3 center { get; set; }

  //Unit SphereTile
  public SphereTile(Vector3 c)
  {
    center = c;
    side = Vector3.one;
    neighbors = new List<SphereTile>();
    triangles = new List<Triangle>();
    subTriangles = new List<Triangle>();
  }
  //A hexagonal SphereTile is made up of 12 triangles, 6 which make the hexagon face and 6 which connect each vertex to the origin (0,0,0)
  public SphereTile(Vector3 c, Vector3 s)
  {
    center = c;
    side = s;
  }
  /*or a SphereHex can be made explicitly with vectors
  public SphereHex(Vector3 one, Vector3 two, Vector3 three, Vector3 four, Vector3 five, Vector3 six)
  {
    v1 = one;
    v2 = two;
    v3 = three;
    v4 = four;
    v5 = five;
    v6 = six;
    center = (v1+v2+v3+v4+v5+v6) / 6;
  }
  */
  /*
  public List<Triangle> ToRender()
  {
    List<Triangle> hexTris = new List<Triangle>();
    hexTris.Add(new Triangle(center, ne, e));
    hexTris.Add(new Triangle(center, e, se));
    hexTris.Add(new Triangle(center, se, sw));
    hexTris.Add(new Triangle(center, sw, w));
    hexTris.Add(new Triangle(center, w, nw));
    hexTris.Add(new Triangle(center, nw, ne));
    return hexTris;
  }
  */
  //Given the subdivided triangles, build the spheretile, which is made up of 12 triangles. 6 face, 6 side
  public void Build()
  {
    Triangle startingAt = subTriangles[0];

    Triangle triCopy = new Triangle(subTriangles[0].v1, subTriangles[0].v2, subTriangles[0].v3);
    //Transform triTrans = this.subTriangles[0].transform;
    Transform triCopyTrans = triCopy.trans;
  
 
    //In case any triangles got added to this that weren't supposed to:
    for (int i = 6; i < subTriangles.Count; i++)
    {
      subTriangles.Remove(subTriangles[i]);
    }
    //Do this next thing 6 times to get 6 triangles
    int z = 0;
    List<float> subs = new List<float>();
    while (z < 6)
    {
      //Rotate our tester to where we want it, check for triangle here
      triCopyTrans.RotateAround(center, center, 60);
      //Get the list of triCopyTrans distance vectors and sort it
      
      subs.Clear();
      foreach (Triangle s in subTriangles)
      {
        subs.Add((s.center - triCopyTrans.position).sqrMagnitude);
      }
      subs.Sort();
      foreach (Triangle t in subTriangles)
      {
        //If this center corresponds to the smallest value in subs
        if ((t.center - triCopyTrans.position).sqrMagnitude == subs[0])
        {
          Triangle faceTri = new Triangle(this.center, startingAt.center, t.center);
          Triangle sideTri = new Triangle(Vector3.zero, faceTri.v3, faceTri.v2);
          triangles.Add(faceTri);
          triangles.Add(sideTri);
          startingAt = t;
        }
      }
      //here we are assuming that we'll always find a triangle in the previous loop
      z++;
    }
  }
}

public class SphereTileComparer : IEqualityComparer<SphereTile>
{
  public bool Equals(SphereTile x, SphereTile y)
  {
    if (x == null || y == null || GetType() != x.GetType() || GetType() != y.GetType())
    {
      return false;
    }
    return x.center == y.center;
  }

  public int GetHashCode(SphereTile st)
  {
    //Check whether the object is null
    if (Object.ReferenceEquals(st, null)) return 0;
    //Get hash code for the Name field if it is not null.
    int hashSphereTileCenter = st.center == null ? 0 : st.center.GetHashCode();
    return hashSphereTileCenter;
  }
}