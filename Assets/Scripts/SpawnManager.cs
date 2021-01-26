using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlatformWay
{
    public GameObject ground;
    public Transform startPoint;
    public Transform endPoint;
    public int numberOfPlatforms = 0;

    public Vector2 platformScale = new Vector2 (1f, 1f);
    public float verticalMin = -6f;
    public float verticalMax = 6f;

    public float heightMax = 20f;
    public float heightMin = -12f;
}

public class SpawnManager : MonoBehaviour 
{
    [SerializeField]
    private PlatformWay[] platformWay;

    private Vector2 originPosition;
    private Vector2 randomPosition;
    private float distanceBetweenPlatforms;

	// Use this for initialization
	void Start ()
    {
        for (int j = 0; j < platformWay.Length; j++)
        {
            Vector2 startPosition = platformWay[j].startPoint.position;
            Vector2 endPosition = platformWay[j].endPoint.position;;

            originPosition = startPosition;
            randomPosition = originPosition;
            
            distanceBetweenPlatforms = (endPosition.x - startPosition.x) / (platformWay[j].numberOfPlatforms + 1);

            for (int i = 1; i <= platformWay[j].numberOfPlatforms; i++)
            {
                randomPosition += new Vector2 (distanceBetweenPlatforms, Random.Range (platformWay[j].verticalMin, platformWay[j].verticalMax));

                // We'd like the last platforms to lead to end position
                if (i == platformWay[j].numberOfPlatforms - 1)
                    randomPosition = new Vector2 (randomPosition.x, -(endPosition.y - originPosition.y)/3 * 2);
                else if (i == platformWay[j].numberOfPlatforms)
                    randomPosition = new Vector2 (randomPosition.x, -(endPosition.y - originPosition.y)/3 * 1);
                else
                {
                    // We don't want our platforms to spawn outside the background range (in Y axis)
                    if (randomPosition.y < platformWay[j].heightMin)
                        randomPosition += new Vector2 (0, platformWay[j].verticalMax);
                    else if (randomPosition.y > platformWay[j].heightMax)
                        randomPosition -= new Vector2 (0, platformWay[j].verticalMax);
                }
                
                GameObject platform = Instantiate (platformWay[j].ground, randomPosition, Quaternion.identity);
                platform.transform.SetParent (this.transform);
                platform.transform.localScale = (Vector3) platformWay[j].platformScale;

                originPosition = randomPosition;
            }
        }
	}
}
