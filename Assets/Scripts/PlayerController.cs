using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
    [HideInInspector]
    public Vector2 activeSpawnPoint;
    public bool facingRight = true; // direction we're facing

    // 'jump' is gonna be true for the frame in which spacebar has been pressed
    private bool jump = true;

    // Binary values for determining if we used double jump
    private bool doubleJumpUsed = false;
    private bool firstJumpUsed = false;

    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
	public float deathTrigger = -40f;
    public float movementDelay = 1.5f;
    public float lightningSpawnOffset = 12f;
    public float lightningRelativeSize = 4f;

    // We will use this for checking if player is standing on the ground
    public Transform GroundCheckL;
    public Transform GroundCheckR;

    private bool grounded = false;
    private bool movementEnabled = true;

    // Used for storing components for animator (check method below)
    private Animator anim;
    // Used for storing reference to a rigid body (check method below)
    private Rigidbody2D rb2d;

    private PlayerStats playerStats;
    private AudioManager audioManager;
	private LoadingManager loadingManager;
    private Transform firePoint;
    private SpriteRenderer bubble;
    
    private float h;

    // for flipping the sprites (wizard's and lightning's)
    private Vector3 theScale;

    // for shooting
    public GameObject fireball;
    public GameObject lightning;

	// Use this for initialization
	void Awake ()
    {
        activeSpawnPoint = transform.position;
        theScale = transform.localScale;

        audioManager = AudioManager.instance;
        if (audioManager == null)
            Debug.LogError ("No AudioManager found in the scene.");

        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();

        bubble = GetComponent<SpriteRenderer>();
        bubble.enabled = false;

        firePoint = transform.FindChild ("HandLeft/FirePoint");
        if (firePoint == null)
            Debug.LogError ("No firePoint has been found.");

        // Passive magicka regeneration
        InvokeRepeating ("RegenerateMagicka", 1f, playerStats.magickaRegen);

        // Checking for ground
        InvokeRepeating ("CheckIfGrounded", 0.05f, 0.05f);
    }

    void Start ()
    {
        loadingManager = LoadingManager.instance;
        if (loadingManager == null)
            Debug.LogError("No LoadingManager found in the scene.");
    }

	// Update is called once per frame
	void Update ()
    {
        // Double jumping
        if (Input.GetButtonDown ("Jump") && !firstJumpUsed && !doubleJumpUsed)
        {
            jump = true;
            firstJumpUsed = true;
        }
        else if (Input.GetButtonDown ("Jump") && firstJumpUsed && !doubleJumpUsed)
        {
            jump = true;
            doubleJumpUsed = true;
        }

        // Falling down
        if (this.transform.position.y <= deathTrigger)
            playerStats.Health -= 10;

        // Shooting
        if (Input.GetButtonDown ("Fire1"))
		{
			if (playerStats.Magicka > 0)
			{
                anim.SetTrigger ("Attack");
                audioManager.PlaySound ("FireballCast");
				playerStats.Magicka -= 1;
				GameObject _projectile = Instantiate (fireball, firePoint.transform.position, Quaternion.identity);
                _projectile.SendMessage ("NameDirection", facingRight);
                // Using SendMessage to call a method in script attached to projectile
                // and pass a parameter with direction to it
			}
            else audioManager.PlaySound ("NoMagicka");
        }

        // Lightning
        if (Input.GetButtonDown ("Fire2"))
        {
            if (playerStats.Magicka >= 8)
            {
                anim.SetTrigger ("Attack");
                playerStats.Magicka -= 8;

                lightning.transform.localScale = lightningRelativeSize * theScale;

                Instantiate (lightning, firePoint.transform.position, Quaternion.identity);
            }
            else audioManager.PlaySound ("NoMagicka");
        }

        // Quitting to main menu
        if (Input.GetButtonDown ("Cancel"))
            loadingManager.LoadMainMenu();
    }

    // Physics code
    void FixedUpdate ()
    {
        // This is going to be a value between -1 and 1, depending on keyboard input
        h = Input.GetAxis ("Horizontal");

        // Whether we're moving left or right, we're using positive value to set the speed
        anim.SetFloat ("Speed", Mathf.Abs(h));

        // Wizard will not move if he's not visible on screen 
        // ( avoiding accidental suicides :-) )
        if (movementEnabled)
        {
            // We're using only right, but it doesn't matter since 'h' can be either positive or negative
            if (h * rb2d.velocity.x < maxSpeed)
            //rb2d.AddForce(Vector2.right * h * moveForce);
                rb2d.velocity = new Vector2 (h * maxSpeed, rb2d.velocity.y);

            // Check if we start going too fast
            // Mathf.Sign is gonna return -1 or 1 depending on rb2d.velocity.x sign
            if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
                rb2d.velocity = new Vector2 (Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
        }
        else
            rb2d.velocity = new Vector2 (0, rb2d.velocity.y);

        // If we're moving to the right (h>0) and we're not facing right (!facingRight)
        if (h > 0 && !facingRight)
            Flip();
        // Opposite: if we're moving to the left and facing right.
        else if (h < 0 && facingRight)
            Flip();

        if (jump)
        {
            // Jump - parameter from animator, used for jump animation
            anim.SetTrigger ("Jump");
            rb2d.velocity = new Vector2 (rb2d.velocity.x, 0f);
            rb2d.AddForce (new Vector2 (0f, jumpForce));
            jump = false;
        }
    }

    // Flipping our hero's sprite
    void Flip ()
    {
        facingRight = !facingRight;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void KillPlayer ()
    {
        playerStats.Lives -= 1;
        transform.position = activeSpawnPoint;
        rb2d.velocity = new Vector2 (0, 0);
        playerStats.Health = playerStats.maxHealth;
        playerStats.Magicka = playerStats.maxMagicka;
        StartCoroutine (TemporarilyDisableMovement());
    }

    void RegenerateMagicka ()
    {
        if (playerStats.Magicka < playerStats.maxMagicka)
            playerStats.Magicka += 1;
    }

    void CheckIfGrounded ()
    {
        if (Physics2D.Linecast (GroundCheckL.position, GroundCheckR.position, 1 << LayerMask.NameToLayer("Ground")))
            grounded = true;
        else
            grounded = false;

        if (grounded)
        {
            firstJumpUsed = false;
            doubleJumpUsed = false;
        }
    }

    // or OnTriggerEnter2D if we don't want to be physically pushed by enemies
    // (obviously, check Is Trigger in inspector) 
    void OnCollisionEnter2D (Collision2D coll) 
    {
        if (coll.gameObject.name.Contains ("JumpPad"))
        {
            rb2d.AddForce (new Vector2(0f, 2500f));
            audioManager.PlaySound ("JumpPad");
        }
    }

    IEnumerator TemporarilyDisableMovement ()
    {
        playerStats.vulnerable = false;
        bubble.enabled = true;

        movementEnabled = false;
        
        yield return new WaitForSeconds (movementDelay);
        movementEnabled = true;

        yield return new WaitForSeconds (movementDelay);
        playerStats.vulnerable = true;
        bubble.enabled = false;
    }
}
