using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour 
{
    public GameObject pigball;

	public float speed = 5f;
	public float patrolTimeInterval = 1f;
	public float shootTimeInterval = 2f;
    public float playSoundChance = 0.3f;
    public float playSoundPeriod = 1f;

	private bool facingRight = false;
	private Transform firePoint;
    private AudioManager audioManager;
    private Animator anim;
    // (we will need this for setting the animation parameter)

    void Start()
    {
        anim = GetComponent<Animator>();

        firePoint = transform.FindChild ("FirePoint");
		if (firePoint == null) 
            Debug.LogError ("No firePoint has been found.");

        audioManager = AudioManager.instance;
        if (audioManager == null)
            Debug.LogError ("No AudioManager found in the scene.");

        // Flipping pig every certain amount of time (patrol)
        InvokeRepeating ("Flip", patrolTimeInterval, patrolTimeInterval);
        InvokeRepeating ("Shoot", shootTimeInterval, shootTimeInterval);
        InvokeRepeating ("RandomizeSound", playSoundPeriod, playSoundPeriod);
    }

	void Update()
	{
		if (facingRight)
		    transform.Translate (Vector2.right * Time.deltaTime * speed, Space.World);
        else
		    transform.Translate (Vector2.left * Time.deltaTime * speed, Space.World);
	}

    void Shoot()
    {
        anim.SetTrigger ("Shoot");
        GameObject _projectile = Instantiate (pigball, firePoint.transform.position, Quaternion.identity);
        _projectile.SendMessage ("NameDirection", facingRight);
    }

	// Flipping pig's sprite
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void RandomizeSound ()
    {
        if (Random.Range(0f, 1f) > (1f - playSoundChance))
            audioManager.PlaySound ("Pig");
    }
}
