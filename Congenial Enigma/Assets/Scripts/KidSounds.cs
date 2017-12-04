using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidSounds : MonoBehaviour {

	public AudioClip[] ChaseSounds;
	public AudioClip[] GetAwaySounds;

	private bool _clipPlayed;


	public static KidSounds Instance;

	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	public void PlayChase()
	{
		ClipPlayer.Instance.ClipToPlay = RandomChaseSound();
		ClipPlayer.Instance.PlayClip();
//		_clipPlayed = true;
	}

	public void PlayGetaway()
	{
		ClipPlayer.Instance.ClipToPlay = RandomGetAwaySound();
		ClipPlayer.Instance.PlayClip();
	}
	
	AudioClip RandomChaseSound()
	{
		var index = Random.Range(0, ChaseSounds.Length);
		return ChaseSounds[index];
	}

	AudioClip RandomGetAwaySound()
	{
		var index = Random.Range(0, GetAwaySounds.Length);
		return GetAwaySounds[index];
	}
}
