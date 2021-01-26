using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalScreenLoader : MonoBehaviour 
{
	public Transform boss;
	public float delay = 3f;

	void Update ()
	{
		if (boss == null)
			StartCoroutine (LoadFinalScreen());
	}

	IEnumerator LoadFinalScreen ()
    { 
        yield return new WaitForSeconds(delay);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("scene5");
        while (!asyncLoad.isDone) yield return null;
    }
}
