using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public enum Music { MusicMenu, MusicLevel1, MusicLevel2, MusicLevel3,  MusicLevel4, MusicGO, MusicMA };
    public Music music;

    private AudioManager audioManager;

    void Awake ()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
            Debug.LogError ("No AudioManager found in the scene.");
    }

	void Start ()
    {
        audioManager.StopMusic();
        audioManager.PlayMusic (music + "");
    }
}
