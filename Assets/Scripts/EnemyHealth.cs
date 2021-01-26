using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour 
{
	public GameObject explosion;
    public RectTransform bossBar;
	public float explosionDuration = 0.8f;
	public int maxHealth;
    private int _health;

	private PlayerStats playerStats;
    private AudioManager audioManager;

    void Start()
	{
		Health = maxHealth;

        GameObject _g = GameObject.Find ("Wizard");
		if (_g == null) Debug.Log ("Error: Wizard object not found");
        else playerStats = _g.GetComponent<PlayerStats>();

        audioManager = AudioManager.instance;
        if (audioManager == null)
            Debug.LogError ("No AudioManager found in the scene.");
    }

	public int Health
    {
        get { return _health; }
        set
        {
            _health = value;

			if (bossBar != null)
            	bossBar.localScale = new Vector2 ((float)Health/maxHealth, 1);

            if (_health <= 0)
			{
				if (explosion != null)
                	InitiateExplosion();
				else
                    Destroy (gameObject);

				playerStats.Kills += 1;
				playerStats.Score += maxHealth;
			}
        }
    }

	void InitiateExplosion()
	{
		Destroy (gameObject);
		GameObject _e = (GameObject) Instantiate (explosion, transform.position, Quaternion.identity);
		Destroy (_e, explosionDuration);
        audioManager.PlaySound("BigExplosion");
    }
}
