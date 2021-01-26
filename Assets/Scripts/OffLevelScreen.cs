using System.Collections;
using UnityEngine;

public class OffLevelScreen : MonoBehaviour
{
    public float delay = 1f;
    private bool delayed = false;
	private LoadingManager loadingManager;

    void Start ()
    {   
        loadingManager = LoadingManager.instance;
        if (loadingManager == null)
            Debug.LogError ("No LoadingManager found in the scene.");

        StartCoroutine (DelayButtonPressDetection());
    }

	void Update ()
    {
        if (Input.anyKey && delayed)
            loadingManager.LoadMainMenu();
	}

    IEnumerator DelayButtonPressDetection ()
    {
        yield return new WaitForSeconds (delay);
        delayed = true;
    }
}
