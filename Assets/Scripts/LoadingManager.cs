using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;

	public void LoadMainMenu() {StartCoroutine(Load(0));}
    public void LoadNewScene (byte _a) {StartCoroutine(Load(_a));}
    public void GameExit () {Application.Quit();}

	public void LoadCreditsScreen() {StartCoroutine(Load(6));}
	public void LoadGameOverScreen() {StartCoroutine(Load(4));}
	public void LoadVictoryScreen() {StartCoroutine(Load(5));}

	public void LoadLevel1() {StartCoroutine(Load(1));}
	public void LoadLevel2() {StartCoroutine(Load(2));}
	public void LoadLevel3() {StartCoroutine(Load(7));}
	public void LoadLevel4() {StartCoroutine(Load(3));}

    void Awake ()
    {
        /*if (instance != null)
        {
            if (instance != this)
                Destroy (gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad (this);
        }*/

        instance = this;
    }

	IEnumerator Load (byte index)
    {
        // The Application loads the Scene in the background at the same time as the current Scene.
        // This is particularly good for creating loading screens. You could also load the Scene by build //number.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync ("scene" + index);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone) yield return null;
    }
}