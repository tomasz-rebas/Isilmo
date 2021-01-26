using UnityEngine;

public class AMReference : MonoBehaviour
{
    private AudioManager audioManager;

    void Start ()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
            Debug.LogError("No AudioManager found in the scene.");
    }

    public void UpdateMusicVolume ()
    {
        audioManager.UpdateMusicVolume();
    }
}
