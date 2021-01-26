using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour 
{
	public GameObject explosion;
	public float arc = 4f;
    public float explosionDuration = 0.8f;
    public int velocity = 20;
	public int damage = 2;

	private float direction;

	void OnTriggerEnter2D (Collider2D coll) 
	{
		if (coll.gameObject.tag == "Enemy")
		{
			EnemyHealth _enemyHealth = coll.GetComponent<EnemyHealth>();
			_enemyHealth.Health -= damage;
			InitiateExplosion(); 
		}
		else if (coll.gameObject.tag == "Player")
		{
			PlayerStats _playerStats = coll.GetComponent<PlayerStats>();
			_playerStats.Health -= damage;
			InitiateExplosion(); 
		}
        else if (coll.gameObject.tag == "Rocket")
			Destroy (gameObject); // Rocket will be exploding already
	}

	void NameDirection (bool facingRight)
	{
		if (facingRight)
				direction = 1;
			else 
				direction = -1;
	}

	void InitiateExplosion ()
	{
        Destroy (gameObject);
		Instantiate (explosion, transform.position, Quaternion.identity);
    }

	void Update ()
	{
		// .:. Movement section .:.
		// We're using transform.Translate() for moving objects over time
		// It's good because it doesn't take as many calculations as using RigidBody
		// Thanks to Time.deltaTime velocity won't be affected by a framerate
		transform.Translate (Vector2.right * direction * Time.deltaTime * velocity + Vector2.up * Time.deltaTime * arc, Space.World);
	}
}
