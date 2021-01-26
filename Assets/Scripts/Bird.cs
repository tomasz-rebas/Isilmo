using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public GameObject bomb;
    public GameObject bombChild;
    public float dropTriggerX = 1f;
    public float dropTriggerY = 10f;
    public float playSoundChance = 0.3f;
    public float playSoundPeriod = 1f;

    // Movement
    public int velocity = 6;
    public float spiralFactor = 8f;
    public float radianUpdate = 0.02f;
    public float startingPhase = 2f;
    private float rad = 0;

    private Transform wizard;
    private AudioManager audioManager;
    private bool bombDropped = false;

    void Start ()
    {
        wizard = transform.Find("/Wizard");
        if (wizard == null) 
            Debug.LogError ("No wizard has been found.");

        audioManager = AudioManager.instance;
        if (audioManager == null)
            Debug.LogError ("No AudioManager found in the scene.");

        InvokeRepeating ("RandomizeSound", playSoundPeriod, playSoundPeriod);
    }

    void Update ()
    {
        // Movement
		if (rad <= startingPhase * Mathf.PI)
			rad += radianUpdate * Mathf.PI * Time.deltaTime;
		else
			rad = -2f * Mathf.PI;

        Vector2 _v2 = new Vector2 (-velocity, Mathf.Sin(rad) / spiralFactor);
        transform.Translate (_v2 * Time.deltaTime, Space.World);

        // Bombing
        if (!bombDropped)
        {
            float _x = Mathf.Abs (gameObject.transform.position.x - wizard.position.x);
            float _y = Mathf.Abs (gameObject.transform.position.y - wizard.position.y);
            if (_x <= dropTriggerX && _y <= dropTriggerY) DropBomb();
        }
    }

    void DropBomb ()
    {
        Destroy (bombChild);
        Instantiate (bomb, transform.position + new Vector3(0, -1, 0), Quaternion.identity);
        bombDropped = true;
    }

    void RandomizeSound ()
    {
        if (Random.Range(0f, 1f) > (1f - playSoundChance))
            audioManager.PlaySound ("Crow");
    }
}
