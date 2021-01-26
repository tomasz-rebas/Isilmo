using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour 
{
	// Movement
	public int velocity = 10;
	public int damage = 4;
	public float spiralFactor = 4f;
	public float spiralFactorRandomization = 0f;
	public float radianUpdate = 0.01f;
	public float radianUpdateRandomization = 0f;
	public float startingPhase = -2f;
	//public float startingPhaseRandomization = 0f;
	private float rad = 0;

    public float playSoundChance = 0.3f;
    public float playSoundPeriod = 1f;

	private AudioManager audioManager;

	void Start ()
	{
		audioManager = AudioManager.instance;
        if (audioManager == null)
            Debug.LogError ("No AudioManager found in the scene.");

		spiralFactor += Random.Range (-spiralFactorRandomization, spiralFactorRandomization);
		radianUpdate += Random.Range (-radianUpdateRandomization, radianUpdateRandomization);
		//startingPhase += Random.Range (-startingPhaseRandomization, startingPhaseRandomization);

        InvokeRepeating ("RandomizeSound", playSoundPeriod, playSoundPeriod);
	}

	void Update ()
	{
		if (rad <= startingPhase * Mathf.PI)
			rad += radianUpdate * Mathf.PI * Time.deltaTime;
		else
			rad = -2f * Mathf.PI;

        Vector2 _v2 = new Vector2 (-velocity, Mathf.Sin(rad) / spiralFactor);
        transform.Translate (_v2 * Time.deltaTime, Space.World);
    }

	void OnCollisionEnter2D (Collision2D coll) 
    {
        if (coll.collider.gameObject.tag == "Player")
		{
			PlayerStats _playerStats = coll.collider.GetComponent<PlayerStats>();
			_playerStats.Health -= damage;
		}
    }

	void RandomizeSound ()
    {
        if (Random.Range(0f, 1f) > (1f - playSoundChance))
            audioManager.PlaySound ("Bee");
    }
}
