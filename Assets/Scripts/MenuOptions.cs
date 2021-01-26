using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptions : MonoBehaviour
{
	public Slider soundSlider;
	public Slider musicSlider;
	public Slider difficultySlider;

	public void Start()
    {
		soundSlider.normalizedValue = Globals.VOLUME_SOUND;
		musicSlider.normalizedValue = Globals.VOLUME_MUSIC;
		difficultySlider.normalizedValue = Globals.DIFFICULTY - 0.5f;

        //Adds a listener to the main slider and invokes a method when the value changes.
        soundSlider.onValueChanged.AddListener (delegate {ChangeSoundVolume(); });
        musicSlider.onValueChanged.AddListener (delegate {ChangeMusicVolume(); });
        difficultySlider.onValueChanged.AddListener (delegate {ChangeDifficulty(); });
    }

    // Invoked when the value of the slider changes.
    public void ChangeSoundVolume () 
	{
		Globals.VOLUME_SOUND = soundSlider.normalizedValue;
	}

    public void ChangeMusicVolume () 
	{
		Globals.VOLUME_MUSIC = musicSlider.normalizedValue;
	}

    public void ChangeDifficulty () 
	{
		Globals.DIFFICULTY = difficultySlider.normalizedValue + 0.5f;
	}
}