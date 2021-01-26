using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject Explosion;
    public float explosionDuration = 0.8f;
    public int damage = 9;

    void OnTriggerEnter2D(Collider2D coll) 
    {
        if (coll.gameObject.tag == "Player")
		{
			PlayerStats _playerStats = coll.GetComponent<PlayerStats>();
			_playerStats.Health -= damage;
			InitiateExplosion(); 
		}
        else if (coll.gameObject.tag == "ground")
			InitiateExplosion(); 
    }

    void InitiateExplosion()
    {
        Destroy(gameObject);
        GameObject explosion = (GameObject)Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(explosion, explosionDuration);
    }
}