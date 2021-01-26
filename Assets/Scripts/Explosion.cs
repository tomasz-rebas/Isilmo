using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float duration = 0.8f;
    private AudioManager audioManager;

    void Start ()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
            Debug.LogError ("No AudioManager found in the scene.");

        audioManager.PlaySound ("Explosion");
        Destroy (gameObject, duration);
    }
}
