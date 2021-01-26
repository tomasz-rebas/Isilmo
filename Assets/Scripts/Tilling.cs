using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]

public class Tilling : MonoBehaviour 
{
	// for handling weird errors
	public int offsetX = 2;

	// for instantiating clone sprites
	public bool spriteOnTheLeft = false;
	public bool spriteOnTheRight = false;

	// used for non-tilable objects
	public bool reverseScale = false;

	// the width of the element
	private float spriteWidth = 0f;
	private Camera cam;
	// storing object's transform in a variable for performance reasons
	private Transform myTransform;

	void Awake ()
	{
		cam = Camera.main;
		myTransform = transform;
	}

	// Use this for initialization
	void Start () 
	{
		// GetComponent is gonna get the first component of a given type.
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
		// It will give us the width of the element.
		spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!spriteOnTheLeft || !spriteOnTheRight)
		{
			// calculating half the width of the main camera view
			float camHorizontalExtend = cam.orthographicSize * Screen.width/Screen.height;
			// calculating the X coordinate where the camera can see the edge of the sprite
			float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth/2) - camHorizontalExtend;
			float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth/2) + camHorizontalExtend;

			// edgeVisiblePositionRight/Left - edges where elements are still visible
			// cam.transform.position.x - position of the camera

			// if camera is positioned further than edge of visibility and there's no sprite
			// on the right, instantiate new sprite
			if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && spriteOnTheRight == false)
			{
				InstantiateNewSprite(1);
				spriteOnTheRight = true;
			}
			else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && spriteOnTheLeft == false)
			{
				InstantiateNewSprite(-1);
				spriteOnTheLeft = true;
			}
		}
	}

	// function for spawning sprites on the sides
	void InstantiateNewSprite (int rightOrLeft)
	{
		// Vector3 myVector = new Vector3 (myTransform.position.x, myTransform.position.y, myTransform.position.z);
		// calculating the position of our new background
		Vector3 newPosition = new Vector3 (myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
		// using "(Transform)" for type conversion
		Transform newSprite = (Transform) Instantiate (myTransform, newPosition, myTransform.rotation);

		// for non-tillable objects
		// it's basically for creating mirror image of a tile (good for looping)
		if (reverseScale == true)
			newSprite.localScale = new Vector3 (newSprite.localScale.x*-1, newSprite.localScale.y, newSprite.localScale.z);

		// that's for parenting so there's no mess in our hierarchy
		newSprite.parent = myTransform.parent;

		// freshly instantiated sprite has to know that there's another sprite already on the left/right
		if (rightOrLeft > 0)
			newSprite.GetComponent<Tilling>().spriteOnTheLeft = true;
		else
			newSprite.GetComponent<Tilling>().spriteOnTheRight = true;
	}

}
