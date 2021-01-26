using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour 
{
	public GameObject explosion;
	public int damage = 16;
	public float ttl = 1f;
    public float explosionDuration = 0.8f;

    private AudioManager audioManager;

	void Start () 
	{
        audioManager = AudioManager.instance;
        if (audioManager == null)
            Debug.LogError("No AudioManager found in the scene.");

        audioManager.PlaySound ("Lightning");

        StartCoroutine (Dismiss());
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
        if (coll.gameObject.tag == "Enemy")
        //if (coll.gameObject.layer == 10)
        {
			EnemyHealth _enemyHealth = coll.GetComponent<EnemyHealth>();
			_enemyHealth.Health -= damage;
			GameObject _e = (GameObject) Instantiate (explosion, coll.transform.position, Quaternion.identity);
			Destroy (_e, explosionDuration);
        }
	}

	IEnumerator Dismiss ()
	{
        yield return new WaitForSeconds (ttl);
		Destroy (gameObject);
	}
}
