using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class AdultController : MonoBehaviour
{

	public AudioClip[] ProtectedClips;
	public bool Leaving = false;

	private bool _playedProtectedClip;
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, 2);
	}

	public void StartTimer()
	{
		StartCoroutine(CountdownToLeaving());
	}
	
	IEnumerator CountdownToLeaving(){
		{
			yield return new WaitForSeconds(5);
			Leaving = true;
		}
	}

	public void PlayProtectedClip()
	{
		if (!_playedProtectedClip)
		{
			ClipPlayer.Instance.ClipToPlay = RandomProtectedSound();
			ClipPlayer.Instance.PlayClip(true);
		}
		_playedProtectedClip = true;
	}
	
	AudioClip RandomProtectedSound()
	{
		var index = Random.Range(0, ProtectedClips.Length - 1);
		return ProtectedClips[index];
	}
	
	void Update()
	{
		if (Leaving)
		{
			Destroy(gameObject);
			// move to ... somewhere?
		}
	}
}
