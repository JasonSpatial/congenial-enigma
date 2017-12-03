using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class AdultController : MonoBehaviour
{

	public bool Leaving = false;
	
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

	void Update()
	{
		if (Leaving)
		{
			Destroy(gameObject);
			// move to ... somewhere?
		}
	}
}
