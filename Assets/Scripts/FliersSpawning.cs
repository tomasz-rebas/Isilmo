using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FliersSpawning : MonoBehaviour
{
    public GameObject flier;
    public GameObject flierSpawnPoint;
    public float yMin = -3f;
    public float yMax = 3f;
    public float intervalMin = 1.1f;
    public float intervalMax = 1.7f;

    void Start ()
    {
        StartCoroutine (SpawnFlier());
    }

    IEnumerator SpawnFlier ()
    {
        while (true)
        {
            yield return new WaitForSeconds (Random.Range(intervalMin, intervalMax));

            Vector3 _pos = Camera.main.gameObject.transform.position;
            // Using position of empty game object attached to the camera

            _pos += new Vector3 (flierSpawnPoint.transform.position.x, Random.Range(yMin, yMax), 1.5f);

            Instantiate (flier, _pos, Quaternion.identity);
        }
    }
}
