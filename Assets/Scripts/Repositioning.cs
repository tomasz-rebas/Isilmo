using UnityEngine;

public class Repositioning : MonoBehaviour 
{
	public Transform spawnPoint;
	public enum Unit { Snake, Hornet }
	public Unit unit;

	void OnTriggerEnter2D (Collider2D coll)
	{   
		switch (unit)
		{
			case Unit.Snake: 
				if (coll.gameObject.name.Contains ("Snake"))
					coll.transform.position = spawnPoint.position;
				break;
			case Unit.Hornet: 
				if (coll.gameObject.name.Contains ("Hornet"))
					coll.transform.position = spawnPoint.position;
				break;
			default: break;
		}
	}
}