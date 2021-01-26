using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffCameraEraser : MonoBehaviour 
{
	public float offset = 0.5f;

	void Update()
	{
		// Camera.main.WorldToScreenPoint(transform.position) transforms position from world space into screen space.

		if (
			(Camera.main.WorldToScreenPoint(transform.position).x >= Screen.width + offset * Screen.width) || 
			(Camera.main.WorldToScreenPoint(transform.position).x <= -offset * Screen.width) ||
			(Camera.main.WorldToScreenPoint(transform.position).y >= Screen.height + offset * Screen.height) || 
			(Camera.main.WorldToScreenPoint(transform.position).y <= -offset * Screen.height)
		   )
		{
			Destroy(gameObject);
			//Debug.Log("Object has gone out of camera's reach and has been destroyed.");
		}
	}
}
