using UnityEngine;
using System.Collections;

public class SpawnCoins : MonoBehaviour {

    // Array of transforms
    public Transform[] spawnPoints;

    public GameObject coin;
    public GameObject diamond;
    public GameObject health;
    public GameObject magicka;
    public GameObject life;

    public float coinChance = 0.5f;
    public float diamondChance = 0.05f;
    public float healthChance = 0.02f;
    public float magickaChance = 0.03f;
    public float lifeChance = 0.005f;

    public bool rarePickup;

	// Use this for initialization
	void Start ()
    {
        // "_t" stands for "threshold"
        float[] _t = new float[6];

        _t[0] = 0f;
        _t[1] = coinChance;
        _t[2] = coinChance + diamondChance;
        _t[3] = coinChance + diamondChance + healthChance;
        _t[4] = coinChance + diamondChance + healthChance + magickaChance;
        _t[5] = coinChance + diamondChance + healthChance + magickaChance + lifeChance;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (rarePickup)
            {
                float _r = Random.Range(0f, 1f);

                if (IsWithin(_r, 0.00f, 0.25f))
                    InstantiatePickup (life, i);
                else if (IsWithin(_r, 0.25f, 0.50f))
                    InstantiatePickup (diamond, i);
                else if (IsWithin(_r, 0.50f, 0.75f))
                    InstantiatePickup (health, i);
                else if (IsWithin(_r, 0.75f, 1.00f))
                    InstantiatePickup (magicka, i);
            }
            else
            {
                float _r = Random.Range(0f, 1f);

                if (IsWithin(_r, _t[0], _t[1]))
                    InstantiatePickup (coin, i);
                else if (IsWithin(_r, _t[1], _t[2]))
                    InstantiatePickup (diamond, i);
                else if (IsWithin(_r, _t[2], _t[3]))
                    InstantiatePickup (health, i);
                else if (IsWithin(_r, _t[3], _t[4]))
                    InstantiatePickup (magicka, i);
                else if (IsWithin(_r, _t[4], _t[5]))
                    InstantiatePickup (life, i);
            }

        }
	}

    void InstantiatePickup (GameObject _pickup, int _i)
    {
        GameObject _go;
        _go = Instantiate(_pickup, spawnPoints[_i].position, Quaternion.identity);
        _go.transform.SetParent(this.transform);
    }

    public bool IsWithin (float _a, float _min, float _max)
    {
        return _a >= _min && _a < _max;
    }
}
