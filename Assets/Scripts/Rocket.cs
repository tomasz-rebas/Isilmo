using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject explosion;
    public float explosionDuration = 0.8f;
    public float speed = 9f;
    public float rotationSpeedModifier = 70.0f;
    public float homingActivationTime = 2f;
    public float ttl = 30;
    public int damage = 3;
    private Transform wizard;
    private Transform boss;
    private Transform rit;
    private bool homing = false;

    void Start ()
    {
        wizard = transform.Find ("/Wizard");
        if (wizard == null) Debug.LogError ("No wizard has been found.");
        boss = transform.Find ("/Boss");
        if (boss == null) Debug.LogError ("No boss has been found.");
        rit = transform.Find ("/Boss/RocketInitialTarget");
        if (rit == null) Debug.LogError ("No RocketInitialTarget has been found.");

        StartCoroutine (ActivateHoming());
    }

	void Update ()
    {
        Vector3 _relative;

        if (homing) _relative = transform.InverseTransformPoint (wizard.position);
        else _relative = transform.InverseTransformPoint (rit.position);

        float angle = Mathf.Atan2(_relative.x, _relative.z) * Mathf.Rad2Deg;
        transform.Rotate(0, 0, (angle / rotationSpeedModifier) * Time.deltaTime);
        transform.Translate(-transform.up * speed * Time.deltaTime, Space.World);

        if (boss == null)
            InitiateExplosion();
    }

    IEnumerator ActivateHoming ()
    {
        yield return new WaitForSeconds (homingActivationTime);
        homing = true;
        yield return new WaitForSeconds (ttl);
        InitiateExplosion();
    }

    void OnTriggerEnter2D (Collider2D coll) 
    {
        if (coll.gameObject.tag == "Player")
		{
			PlayerStats _playerStats = coll.GetComponent<PlayerStats>();
			_playerStats.Health -= damage;
			InitiateExplosion(); 
		}
        else if (coll.gameObject.tag == "Bullet")
			InitiateExplosion(); 
    }

    void InitiateExplosion ()
    {
        Destroy (gameObject);
        GameObject _e = (GameObject) Instantiate (explosion, transform.position, Quaternion.identity);
        Destroy (_e, explosionDuration);
    }
}
