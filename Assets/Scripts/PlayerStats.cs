using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // byte: 0 to 255, int: -127 to 127 (more useful here)
	public readonly int maxLives = 3;

	public int maxHealth = 10;
	public int maxMagicka = 10;
	public float magickaRegen = 1.5f;

    [HideInInspector]
    public bool vulnerable = true;

    private int _lives = 3;
    private int _health;
    private int _magicka;
    private int _score = 0;
    private int _kills = 0;
    private int _items = 0;
    private int _maxItems = 0;

    private GameHUD gameHUD;
    private PlayerController playerController;
    private AudioManager audioManager;
    private LoadingManager loadingManager;

    void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
            Debug.LogError ("No AudioManager found in the scene.");

		loadingManager = LoadingManager.instance;
        if (loadingManager == null)
            Debug.LogError ("No LoadingManager found in the scene.");

        GameObject _g;
		
		_g = GameObject.Find ("UIOverlay");
		if (_g == null) Debug.Log ("Error: UIOverlay object not found");
		else gameHUD = _g.GetComponent<GameHUD>();

        _g = GameObject.Find ("Wizard");
		if (_g == null) Debug.Log ("Error: Wizard object not found");
        else playerController = _g.GetComponent<PlayerController>();

        // Difficulty related adjustments
        maxHealth = (int) (maxHealth / Globals.DIFFICULTY);
        _health = maxHealth;
        maxMagicka = (int) (maxMagicka / Globals.DIFFICULTY);
        if (maxMagicka < 8) maxMagicka = 8;
        _magicka = maxMagicka;
    }

    public int Lives
    {
        get { return _lives; }
        set
        {
            if (value < 0)
            loadingManager.LoadGameOverScreen();
            else if (value <= 3)
            {
                _lives = value;
                gameHUD.UpdateLives();
            }

        }
    }

    public int Health
    {
        get { return _health; }
        set
        {
            if (vulnerable)
            {
                if (_health > value)
                {
                    StartCoroutine (gameHUD.RedFlash());
                    audioManager.PlaySound ("Hit");
                }

                _health = value;
                gameHUD.UpdateHealth();

                if (_health <= 0)
                    playerController.KillPlayer();
            }
        }
    }

    public int Magicka
    {
        get { return _magicka; }
        set
        {
            _magicka = value;
            gameHUD.UpdateMagicka();
        }
    }

    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            gameHUD.UpdateScore();
        }
    }

    public int Kills
    {
        get { return _kills; }
        set
        {
            _kills = value;
            gameHUD.UpdateKills();
        }
    }

    public int Items
    {
        get { return _items; }
        set
        {
            _items = value;
            gameHUD.UpdateItems();
        }
    }

    public int MaxItems
    {
        get { return _maxItems; }
        set
        {
            _maxItems = value;
        }
    }

    // 'static' means that we can get access to the content without making an instance
    // of particular method/class.
    // " : MonoBehaviour" means that UIUpdater inherits from MonoBehaviour class
    // and thus HAS to be added as a component to some of my GameObjects.
    // There cannot be any instances of classes that inherit from
    // MonoBehaviour. That's why we use static methods so we can
    // call them without an instance.
    //
    // Different approach: if you want to use variables or methods from
    // MonoBehaviour-inheriting class, find the object the script is
    // attached to and use GetComponent() methods (check out PlayerStats.cs
    // for reference).
}
