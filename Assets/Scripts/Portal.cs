using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour 
{
	public byte nextSceneIndex = 0;
	public bool gate = false;

    private AudioManager audioManager;
	private LoadingManager loadingManager;

	void Start ()
	{	
		audioManager = AudioManager.instance;
        if (audioManager == null)
            Debug.LogError ("No AudioManager found in the scene.");

		loadingManager = LoadingManager.instance;
        if (loadingManager == null)
            Debug.LogError ("No LoadingManager found in the scene.");
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.name.Contains ("Wizard"))
		{
			if (gate)
				audioManager.PlaySound ("Gate");
			else
				audioManager.PlaySound ("Portal");

			loadingManager.LoadNewScene (nextSceneIndex);
		}
	}
}
