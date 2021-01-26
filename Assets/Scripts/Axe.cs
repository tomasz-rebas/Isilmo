using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour 
{
	public int damage = 2;
    private PlayerStats playerStats;

	void Start () 
	{
		GameObject wizard;

        wizard = GameObject.Find ("/Wizard");
        if (wizard == null) Debug.LogError ("No wizard has been found.");
		
        playerStats = wizard.GetComponent<PlayerStats>();
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
        if (coll.gameObject.name.Contains("Wizard"))
            playerStats.Health -= damage;
	}
}
