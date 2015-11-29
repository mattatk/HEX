using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LibNoise.Unity;
using LibNoise.Unity.Generator;
using LibNoise.Unity.Operator;

//Each SphereTile is a piece of the dual polysphere,
//all together they are the dual polysphere
//It's still a mystery to me as to why the pentagons seem to get made nicely and whether or not there's something wrong with them
//@TODO: Solution for skewing or is it fine??
public class SphereTile : PolySphere
{
  public bool colliding; //OnCollisionStay
  public TileType type;
  //The inital triangles from the subdivided polysphere which we will use to build the spheretile
  public List<Triangle> subTriangles;
  //The triangles that make up this piece of the entire dual polygon
  public List<Triangle> faceTris;
  public List<Triangle> sideTris;
  //Neighbor dual faces
  public List<SphereTile> neighbors;
  //GameObject stuff
  public GameObject stuff;
  //Checking equality with the center vertex 
  private Vector3 _center;
  public Vector3 center{ get; set; }
  //Scaling property
  private float _scale = 1;
  public float scale
  {
    get { return _scale; }
    set
    {
      _scale = value;
      center.Normalize();
      center *= scale;
      foreach (Triangle t in faceTris)
      {
        t.v1.Normalize();
        t.v1 *= scale;
        t.v2.Normalize();
        t.v2 *= scale;
        t.v3.Normalize();
        t.v3 *= scale;
        t.center.Normalize();
        t.center *= scale;
        t.trans.position.Normalize();
        t.trans.position *= scale;
      }
      foreach (Triangle t in sideTris)
      {
        t.v1.Normalize();
        t.v1 *= scale;
        t.v2.Normalize();
        t.v2 *= scale;
        t.v3.Normalize();
        t.v3 *= scale;
        t.center.Normalize();
        t.center *= scale;
        t.trans.position.Normalize();
        t.trans.position *= scale;
      }
    }
  }

  

  //Unit SphereTile
  public SphereTile(Vector3 c)
  {
    center = c;
    neighbors = new List<SphereTile>();
    faceTris = new List<Triangle>();
    sideTris = new List<Triangle>();
    subTriangles = new List<Triangle>();
  }
  //Given the subdivided triangles, build the spheretile, which is made up of 12 triangles. 6 face, 6 side
  public void Build(SimplexNoise simplex)
  {
    //simplex = new SimplexNoise(GameManager.gameSeed);
    List<Triangle> subCopies = new List<Triangle>(subTriangles);
    //unit vector in the direction of subCopies[0].center. Each vector will have the same scaling factor, so we only need to do this once
    Vector3 dir = subCopies[0].center / subCopies[0].center.magnitude;
    //center gives us both a point and normal for our face plane
    //Parameter (D) determined by plane equation
    float planeParam = -center.x * center.x - center.y * center.y - center.z * center.z;
    //Scaling factor determined by plane equation parameters -> s(Ax + By + Cz) + D = 0
    float s = (-planeParam) / (center.x * dir.x + center.y * dir.y + center.z * dir.z);
    //Scale our vectors to be on the face plane
    foreach (Triangle t in subCopies)
    {
      t.center = (t.center / t.center.magnitude) * s; //Could use t.center.Normalize() * s
    }

    Triangle startingAt = subCopies[0];
    Triangle triCopy = new Triangle(subCopies[0].v1, subCopies[0].v2, subCopies[0].v3);
    Transform triCopyTrans = triCopy.trans;
    int z = 0;
    List<float> subs = new List<float>();

    while (z < 6) //Make our 12 triangles (the pentagons apparently work) @TODO: optimize, scew
    {
      //Rotate our tester to where we want it, check for subTriangle here
      triCopyTrans.RotateAround(center, center, 60);

      //Get the list of triCopyTrans distance vectors and sort it to find the smallest(?)
      subs.Clear();
      foreach (Triangle t in subCopies)
      {
        subs.Add((t.center - triCopyTrans.position).sqrMagnitude);
      }
      subs.Sort();
      foreach (Triangle t in subCopies)
      {
        //If this center corresponds to the smallest value in subs
        if ((t.center - triCopyTrans.position).sqrMagnitude == subs[0])
        {
          Triangle faceTri = new Triangle(center, startingAt.center, t.center);
          Triangle sideTri = new Triangle(Vector3.zero, faceTri.v3, faceTri.v2);
          faceTris.Add(faceTri);
          sideTris.Add(sideTri);
          startingAt = t;
        }
      }
      //here we are assuming that we'll always find a triangle in the previous loop
      z++;
    }
  }
  

  public void Smooth()
  {
    //Alright let's do some neighbor shit
    Vector3 average = Vector3.zero;
    foreach (SphereTile st in neighbors)
    {
      average += st.center;
    }
    average /= neighbors.Count;
    scale = average.magnitude;
  }

  void OnCollisionStay()
  {
    colliding = true;
  }
}




//Compare centers, return true if equal
/*
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

    int hashSphereTileCenter = st.center == null ? 0 : st.center.GetHashCode();
    return hashSphereTileCenter;
  }
}
*/

