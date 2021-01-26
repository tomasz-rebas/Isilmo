using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour 
{
	public int velocity = 10;
	public int damage = 4;
	public float changeDirectionTime = 1f;
    public float playSoundChance = 0.3f;
    public float playSoundPeriod = 1f;

    private float verticalMin;
    private float verticalMax;

    /*private enum Direction { Right, Up, Down }
    private Direction direction = Direction.Right;*/

    private Vector2 direction = Vector2.right;

	private AudioManager audioManager;

	void Start ()
	{
		audioManager = AudioManager.instance;
        if (audioManager == null)
            Debug.LogError ("No AudioManager found in the scene.");

        GameObject _refPointSnakeDown = GameObject.Find("RefPointSnakeDown");
        if (_refPointSnakeDown == null) Debug.Log("RefPointSnakeDown not found.");
        GameObject _refPointSnakeUp = GameObject.Find("RefPointSnakeUp");
        if (_refPointSnakeUp == null) Debug.Log("RefPointSnakeUp not found.");

        verticalMin = _refPointSnakeDown.transform.position.y;
        verticalMax = _refPointSnakeUp.transform.position.y;

        InvokeRepeating ("RandomizeSound", playSoundPeriod, playSoundPeriod);
        InvokeRepeating ("ChangeDirection", changeDirectionTime, changeDirectionTime);
	}

	void Update ()
	{
        transform.Translate (direction * velocity * Time.deltaTime, Space.World);
    }

    void ChangeDirection ()
    {
        float _rng = Random.Range (0f, 1f);

        if (transform.position.y >= verticalMax)
            if (_rng < 0.5f) direction = Vector2.down;
            else direction = Vector2.right;
        else if (transform.position.y <= verticalMin)
            if (_rng < 0.5f) direction = Vector2.up;
            else direction = Vector2.right;
        else if (direction == Vector2.right)
            if (_rng < 0.5f) direction = Vector2.up;
            else direction = Vector2.down;
        else if (direction == Vector2.up)
            if (_rng < 0.5f) direction = Vector2.right;
            else direction = Vector2.down;
        else if (direction == Vector2.down)
            if (_rng < 0.5f) direction = Vector2.right;
            else direction = Vector2.up;
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
