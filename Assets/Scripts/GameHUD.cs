using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    public GameObject[] life = new GameObject[3];
    public Image redFlash;
    public RectTransform health;
    public RectTransform magicka;
    public Text score;
    public Text kills;
    public Text items;

	private PlayerStats playerStats;

    void Start()
    {
		GameObject _g;
		
        _g = GameObject.Find ("Wizard");
		if (_g == null) Debug.Log ("Error: Wizard object not found");
        else playerStats = _g.GetComponent<PlayerStats>();
    }

    public void UpdateLives()
    {
        switch (playerStats.Lives)
        {
            case 3:
                life[0].SetActive(true);
                life[1].SetActive(true);
                life[2].SetActive(true);
                break;
            case 2:
                life[0].SetActive(true);
                life[1].SetActive(true);
                life[2].SetActive(false);
                break;
            case 1:
                life[0].SetActive(true);
                life[1].SetActive(false);
                life[2].SetActive(false);
                break;
            case 0:
                life[0].SetActive(false);
                life[1].SetActive(false);
                life[2].SetActive(false);
                break;
        }
    }

    public void UpdateHealth()
    {
        health.localScale = new Vector2 ((float)playerStats.Health/playerStats.maxHealth, 1);
    }

    public void UpdateMagicka()
    {
        magicka.localScale = new Vector2 ((float)playerStats.Magicka/playerStats.maxMagicka, 1);
    }

    public void UpdateScore()
    {
        score.text = "" + playerStats.Score;
    }

    public void UpdateKills()
    {
        kills.text = "" + playerStats.Kills;
    }

    public void UpdateItems()
    {
        int _p = (100 * playerStats.Items) / playerStats.MaxItems;
        items.text = "" + _p;
    }

    public IEnumerator RedFlash()
    {
        Color _c = redFlash.color;

        for (float i = 0.7f; i >= 0; i = i - 0.03f)
        {
            _c.a = i;
            redFlash.color = _c;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
