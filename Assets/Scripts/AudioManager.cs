using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip clip;
    public string name;
    public bool loop = false;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;
    public float randomPitch = 0.1f;

    private AudioSource source;

    public void SetSource (AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play ()
    {
        source.volume = volume * Globals.VOLUME_SOUND * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        source.Play();
    }

    public void Stop ()
    {
        source.Stop();
    }
}

[System.Serializable]
public class Music
{
    public AudioClip clip;
    public string name;
    public bool loop = false;

    [Range(0f, 1f)]
    public float volume = 0.5f;

    private AudioSource source;

    public void SetSource (AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play ()
    {
        source.volume = volume * Globals.VOLUME_MUSIC;
        source.Play();
    }

    public void Stop ()
    {
        source.Stop();
    }

    public void UpdateVolume ()
    {
        source.volume = volume * Globals.VOLUME_MUSIC;
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private Sound[] sounds;
    [SerializeField]
    private Music[] music;

    void Awake ()
    {
        if (instance != null)
        {
            if (instance != this)
                Destroy (gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad (this);
        }
    }

    void Start ()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject ("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent (this.transform);
            sounds[i].SetSource (_go.AddComponent<AudioSource>());
        }

        for (int i = 0; i < music.Length; i++)
        {
            GameObject _go = new GameObject ("Music_" + i + "_" + music[i].name);
            _go.transform.SetParent (this.transform);
            music[i].SetSource (_go.AddComponent<AudioSource>());
        }
    }

    ///////////////////////////////////////////////////
    ///////////////////////////////////////////////////
    ///////////////////////////////////////////////////

    public void PlaySound (string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
                return; 
                // We found the sound so there's no need for more loops
            }

        // No sounds with _name
        Debug.LogWarning ("AudioManager: Sound not found in list: " + _name);
    }

    public void StopSound (string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Stop();
                return;
                // We found the sound so there's no need for more loops
            }
        }

        // No sounds with _name
        Debug.LogWarning ("AudioManager: Sound not found in list: " + _name);
    }

    public void PlayMusic (string _name)
    {
        for (int i = 0; i < music.Length; i++)
            if (music[i].name == _name)
            {
                music[i].Play();
                return; 
                // We found the sound so there's no need for more loops
            }

        // No sounds with _name
        Debug.LogWarning ("AudioManager: Music not found in list: " + _name);
    }

    public void StopMusic ()
    {        
        for (int i = 0; i < music.Length; i++)
            music[i].Stop();
    }

    public void UpdateMusicVolume ()
    {
        for (int i = 0; i < music.Length; i++)
            music[i].UpdateVolume();
    }
}
