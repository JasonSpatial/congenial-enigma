using UnityEngine;

public class ClipPlayer : MonoBehaviour
{

	public AudioClip ClipToPlay;
	public static ClipPlayer Instance = null;

	private AudioSource _audioSource;
	
	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	private void Start()
	{
		_audioSource = GetComponent<AudioSource>();
		_audioSource.loop = false;
	}
	
	public void PlayClip(bool prioritize = false)
	{
		if (_audioSource.isPlaying)
		{
			if (prioritize)
			{
				_audioSource.clip = ClipToPlay;
				_audioSource.Play();
			}
		}
		else
		{
			_audioSource.clip = ClipToPlay;
			_audioSource.Play();
		}
	}
}
