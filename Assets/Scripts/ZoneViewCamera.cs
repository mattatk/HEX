using UnityEngine;
using System.Collections;

public class ZoneViewCamera : MonoBehaviour {

  Transform myTrans;
  float speed = .1f;

	public void Initialize ()
  {
    myTrans = transform;
	}
	
	// Update is called once per frame
	void Update () {
	   if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
        Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
        myTrans.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);

        Debug.Log(touchDeltaPosition);
     }

    if (Input.GetMouseButton(0))
    {
       Vector2 touchDeltaPosition = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
       myTrans.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);
    }
	}
}
