using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour
{
	private PlayerStats playerStats;
    private AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
            Debug.LogError ("No AudioManager found in the scene.");

        GameObject _g;
		
        _g = GameObject.Find ("Wizard");
		if (_g == null) Debug.Log ("Error: Wizard object not found");
        else playerStats = _g.GetComponent<PlayerStats>();

        playerStats.MaxItems ++;
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.CompareTag ("Player"))
        {
            if (gameObject.name.StartsWith ("Coin"))
            {
				playerStats.Score = playerStats.Score + 1;
                PickUpItem ("Coin");
            }
            else if (gameObject.name.StartsWith ("Diamond"))
            {
				playerStats.Score += 10;
                PickUpItem ("Coin");
            }
            else if (gameObject.name.StartsWith ("ExtraLife") && playerStats.Lives < 3)
            {
                playerStats.Lives += 1;
                PickUpItem ("ExtraLife");
            }
            else if (gameObject.name.StartsWith ("Health") && playerStats.Health < playerStats.maxHealth)
            {
                playerStats.Health = playerStats.maxHealth;
                PickUpItem ("Coin");
            }
            else if (gameObject.name.StartsWith ("Magicka") && playerStats.Magicka < playerStats.maxMagicka)
            {
                playerStats.Magicka = playerStats.maxMagicka;
                PickUpItem ("Coin");
            }

            // Object disappears if we take it
        }
    }

    void PickUpItem (string _sound)
    {
        audioManager.PlaySound (_sound);
        Destroy (gameObject);
        playerStats.Items ++;
    }
}
