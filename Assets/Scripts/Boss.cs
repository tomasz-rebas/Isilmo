using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour 
{
    public GameObject rocket;
    public Transform edgeLeft;
    public Transform edgeRight;
    public float walkingSpeed = 6f;
    public float jumpHorizontalSpeed = 15f;
    public float jumpHeightModifier = 0.8f;
    public float jumpRadianUpdate = 0.02f;
    public float meleeRange = 6f;
    public float jumpingChance = 0.1f;
    public float rangeAttackChance = 0.1f;
    public float launchRocketInterval = 0.4f;

    private Transform wizard;
    private Transform firePoint;
    private Transform rit;
    private Animator anim;
    private AudioManager audioManager;
    private bool facingRight = false;
    private bool inMeleeRange;
    private float edgeLeftX;
    private float edgeRightX;
    private float groundLevel;
    private float bossWizardDistance;
    private float rng;
    private float jumpPhase = 0.5f * Mathf.PI;

    private enum Status {Walking, Jumping, MeleeAttacking, RangeAttacking};
    private Status status = Status.Walking;

    // Wizard's position dependant. -1 means left, 1 means right.
    // Minotaur will always be walking towards the wizard.
    // (unless jumping or attacking)
    private sbyte walkingDirection = -1;

    void Start()
    {
        anim = GetComponent<Animator>();
        groundLevel = transform.position.y;

        audioManager = AudioManager.instance;
        if (audioManager == null)
            Debug.LogError("No AudioManager found in the scene.");

        wizard = transform.Find ("/Wizard");
        if (wizard == null) Debug.LogError ("No wizard has been found.");
        rit = transform.FindChild ("RocketInitialTarget");
        if (rit == null) Debug.LogError ("No RocketInitialTarget has been found.");
        firePoint = transform.FindChild ("boss_2_leftArm/FirePoint");
        if (firePoint == null) Debug.LogError ("No firePoint has been found.");

        InvokeRepeating ("RandomizeAction", 3f, 3f);

        // Calculating edges
        edgeLeftX = edgeLeft.position.x;
        edgeRightX = edgeRight.position.x;
    }

    void Update()
    {
        if (status != Status.Jumping)
        {
            CheckDirection();
            CheckDistance();
        }

        switch (status)
        {
            case Status.Walking: Walk(); break;
            case Status.Jumping: Jump();  break;
            case Status.MeleeAttacking: MeleeAttack();  break;
            case Status.RangeAttacking:  break;
        }
    }

    ////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////

    void Walk()
    {
        CheckDistance();
        transform.Translate (new Vector2 (walkingDirection * walkingSpeed * Time.deltaTime, 0));

        if (inMeleeRange)
        {
            status = Status.MeleeAttacking;
            anim.SetBool ("MeleeAttack", true);
            InvokeRepeating ("PlayMeleeSwingSound", 0.3f, 0.8f);
        }
    }

    void Jump()
    {
        Vector2 _v2;

        if (transform.position.x <= edgeLeftX || transform.position.x >= edgeRightX)
            _v2 = new Vector2 (0, Mathf.Sin(jumpPhase) / jumpHeightModifier);
        else
            _v2 = new Vector2 (walkingDirection * jumpHorizontalSpeed, Mathf.Sin(jumpPhase) / jumpHeightModifier);

        transform.Translate (_v2 * Time.deltaTime, Space.World);

        jumpPhase += jumpRadianUpdate * Mathf.PI * Time.deltaTime;
        if (transform.position.y <= groundLevel)
        {
            transform.position = new Vector2 (transform.position.x, groundLevel);
            jumpPhase = 0.5f * Mathf.PI;
            status = Status.Walking;
        }
    }

    void MeleeAttack()
    {
        if (!inMeleeRange)
        {
            status = Status.Walking;
            anim.SetBool ("MeleeAttack", false);
            CancelInvoke ("PlayMeleeSwingSound");
        }
    }

    IEnumerator RangeAttack()
    {
        status = Status.RangeAttacking;
        anim.SetTrigger ("RangeAttack");
        yield return new WaitForSeconds (1.5f * launchRocketInterval);
        audioManager.PlaySound ("RocketLaunch");
        Instantiate(rocket, firePoint.transform.position, rit.transform.rotation);
        yield return new WaitForSeconds (launchRocketInterval);
        audioManager.PlaySound ("RocketLaunch");
        Instantiate(rocket, firePoint.transform.position, rit.transform.rotation);
        yield return new WaitForSeconds (launchRocketInterval);
        audioManager.PlaySound ("RocketLaunch");
        Instantiate(rocket, firePoint.transform.position, rit.transform.rotation);
        yield return new WaitForSeconds (1.5f * launchRocketInterval);
        status = Status.Walking;
    }

    ////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////

    void CheckDirection()
    {
        if (transform.position.x - wizard.transform.position.x > 0 && facingRight)
        {
            Flip();
            walkingDirection = -1;
        }
        else if (transform.position.x - wizard.transform.position.x <= 0 && !facingRight)
        {
            Flip();
            walkingDirection = 1;
        }
    }

    void CheckDistance()
    {
        bossWizardDistance = Mathf.Abs(transform.position.x - wizard.transform.position.x);
        if (meleeRange >= bossWizardDistance) inMeleeRange = true;
        else inMeleeRange = false;
    }

    // Flipping sprite
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void RandomizeAction()
    {
        // 80% for walking
        // 10% for a jump
        // 10% for a rocket launch

        if (status == Status.Walking)
        {
            rng = Random.Range(0.0f, 1.0f);

            if (rng >= 0 && rng <= jumpingChance)
                status = Status.Jumping;
            else if (rng > jumpingChance && rng <= jumpingChance + rangeAttackChance)
                StartCoroutine(RangeAttack());
        }
    }

    void PlayMeleeSwingSound ()
    {
        audioManager.PlaySound ("MeleeSwing");
    }
}