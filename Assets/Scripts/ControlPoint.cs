using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPoint : MonoBehaviour
{
    private Animator animator;
    private AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
            Debug.LogError("No AudioManager found in the scene.");

        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D (Collider2D coll)
    {
        if (coll.gameObject.name.Contains("Wizard"))
        {
            PlayerController _s = coll.GetComponent<PlayerController>();
            _s.activeSpawnPoint = coll.transform.position;
            Destroy (GetComponent<BoxCollider2D>());
            animator.SetTrigger ("Activate");
            audioManager.PlaySound ("ControlPoint");
        }
    }
}