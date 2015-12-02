using UnityEngine;
using System.Collections;

[System.Serializable]
public class Hexagon {
  /*
      w0 - world origin

        2
    1       3
      center
    6       4
        5
  */

  public SerializableVector3 center, v1, v2, v3, v4, v5, v6;

  public Hexagon(Vector3 c, Vector3 one, Vector3 two, Vector3 three, Vector3 four, Vector3 five, Vector3 six)
  {
    center = c;
    v1 = one;
    v2 = two;
    v3 = three;
    v4 = four;
    v5 = five;
    v6 = six;
  }
}
