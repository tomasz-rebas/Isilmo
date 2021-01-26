using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	// Camera will be 'attached' to the player
	public Transform player;

	// 'offset' is a distance between the player object and the camera (Z axis!)
	public float offsetZ = -1.5f;

	// Variables for camera smoothing
	public float damping = 1;
	public float lookAheadFactor = 3;
	public float lookAheadReturnSpeed = 0.5f;
	public float lookAheadMoveThreshold = 0.1f;

	private Vector3 lastPlayerPosition;
	private Vector3 currentVelocity;
	private Vector3 lookAheadPos;

	// These floats will be based on referenced objects
	private float verticalMin;
	private float verticalMax;
	private float horizontalMin;
	private float horizontalMax;

	// Use this for initialization
	void Start () 
	{
		lastPlayerPosition = player.position;

		// Borders will be automatically calculated, based on these objects
		GameObject _camRefPointLeftDown = GameObject.Find ("CamRefPointLeftDown");
		if (_camRefPointLeftDown == null) Debug.Log ("CamRefPointLeftDown not found.");
		GameObject _camRefPointRightUp = GameObject.Find ("CamRefPointRightUp");
		if (_camRefPointRightUp == null) Debug.Log ("CamRefPointRightUp not found.");

		// 'orthographicSize' gives us half a height by default. For the width we need to use aspect ratio
		float halfCamHeight = Camera.main.orthographicSize;
		float halfCamWidth = Camera.main.orthographicSize * Screen.width/Screen.height;

		verticalMin = _camRefPointLeftDown.transform.position.y + halfCamHeight;
		verticalMax = _camRefPointRightUp.transform.position.y - halfCamHeight;
		horizontalMin = _camRefPointLeftDown.transform.position.x + halfCamWidth;
		horizontalMax = _camRefPointRightUp.transform.position.x - halfCamWidth;
	}
	
	// LateUpdate is called once all the instructions from Update are complete
	void LateUpdate ()
	{
		float xMoveDelta = (player.position - lastPlayerPosition).x;
		bool updateLookAheadPlayer = Mathf.Abs (xMoveDelta) > lookAheadMoveThreshold;

		if (updateLookAheadPlayer)
			lookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign (xMoveDelta);
		else
			lookAheadPos = Vector3.MoveTowards (lookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);

		Vector3 aheadPlayerPos = player.position + lookAheadPos + Vector3.forward * offsetZ;
		Vector3 newPos = Vector3.SmoothDamp (transform.position, aheadPlayerPos, ref currentVelocity, damping);

		newPos = new Vector3
		(
			Mathf.Clamp (newPos.x, horizontalMin, horizontalMax), 
			Mathf.Clamp (newPos.y, verticalMin, verticalMax), 
			offsetZ
		);

		transform.position = newPos;
		lastPlayerPosition = player.position;

		// Trying to use "transform.position.z" instead of "player.position.z" seems to cause harmful camera movement in Z axis.
		// "(player.position - offset).y > verticalMin" condition is to keep track of player's position so our camera can move again
		// in Y axis when we go up.
		// Mathf.Clam returns value within certain range
	}
}
