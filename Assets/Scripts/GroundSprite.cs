using UnityEngine;

public class GroundSprite : MonoBehaviour 
{
    public Sprite[] groundSprite;

	void Start () 
	{
		GetComponent<SpriteRenderer>().sprite = groundSprite[(int)Random.Range(0, groundSprite.Length)];
	}
}
