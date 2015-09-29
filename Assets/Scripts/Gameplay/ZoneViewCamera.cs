using UnityEngine;
using System.Collections;

public class ZoneViewCamera : MonoBehaviour {

  float tapRadius = .1f;

  float tapRadiusSquared;
  Transform myTrans;
  float dragSpeed = .3f;
  float zoomSpeed = 50;
  bool dragging;
  Vector2 dragStartPos;

	public void Initialize ()
  {
    myTrans = transform;
    tapRadiusSquared = tapRadius * tapRadius;
	}
	
	void Update ()
  {
    // Begin touch/click
    if (Input.GetMouseButtonDown(0))
    {
      dragging = true;
      dragStartPos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    // Dragging
    if (Input.GetMouseButton(0))
    {  
      Vector2 touchDeltaPosition = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) - dragStartPos;

      if (touchDeltaPosition.sqrMagnitude > tapRadiusSquared)
        myTrans.Translate(-touchDeltaPosition.x * dragSpeed, -touchDeltaPosition.y * dragSpeed, 0);
    }

    // Releasing
    if (Input.GetMouseButtonUp(0))
    {
      Vector2 currentPos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
      Vector2 touchDeltaPosition = currentPos - dragStartPos;
      
      if (touchDeltaPosition.sqrMagnitude < tapRadiusSquared)
        GameManager.OnTapInput(currentPos);
    }

    // Zooming in/out
    float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
    if (scroll != 0)
    {
      myTrans.Translate(Vector3.forward * scroll * Time.deltaTime);
    }
	}
}
