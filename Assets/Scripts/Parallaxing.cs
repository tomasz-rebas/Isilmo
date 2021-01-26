using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour 
{

	// array of background textures
	public Transform[] backgrounds;
	// array of parallax scales (proportions, amount of parallaxing)
	private float[]	parallaxScale;
	// smoothness of parallaxing
	public float smoothing = 1f;
	// a reference to the main camera
	private Transform cam;
	// previous camera position (in the previous frame)
	private Vector3 prevCamPos;

	// This method is being executed between setting objects up and running Start.
	void Awake () 
	{
		// setting up camera reference
		cam = Camera.main.transform;
	}

	// Use this for initialization
	void Start () 
	{
		// The previous frame had the current frame's camera position.
		prevCamPos = cam.position;
		// There is one parallax scale value for each background transform.
		parallaxScale = new float[backgrounds.Length];

		// Creating parallax scales based on backgrounds' positions.
		// We're gonna multiply this by -1 because we need to make parallaxScale
		// inversly proportional to background's z position.
		for (int i = 0; i < backgrounds.Length; i++)
			parallaxScale[i] = backgrounds[i].position.z * (-1);
    }
	
	// Update is called once per frame
	void Update () 
	{
		for (int i = 0; i < backgrounds.Length; i++) 
		{
			float parallax = (prevCamPos.x - cam.position.x) * parallaxScale[i];
			float backgroundTargetPosX = backgrounds[i].position.x + parallax;
			Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
			backgrounds[i].position = Vector3.Lerp (backgrounds[i].position, backgroundTargetPos, smoothing * Time.fixedDeltaTime);
		}

		prevCamPos = cam.position;
	}
}
