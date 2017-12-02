﻿using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public int Purrs;
	public float Speed = 1.0f;
	public float SwipeRate = 0.5f;
	public GameObject PurrSound;
	public GameObject SwipeSound;
	public GameObject AnnoyedSound;
	public GameObject[] PurrMeter;
	public GameObject[] NineLivesMeter;
	
	private Rigidbody2D _rb;
	private float _swipeDelay;
	private Color _originalColor;
	private SpriteRenderer _sprite;
	

	private void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
		_sprite = GetComponentInChildren<SpriteRenderer>();
		_originalColor = _sprite.color;
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetButton("Fire1") && Time.time > _swipeDelay)
		{
			Swipe();
		}
		
		Vector3 rotations = new Vector3(Input.GetAxis("RightH"),0, Input.GetAxis("RightV"));
		if (rotations != Vector3.zero)
		{
			if (Time.time > _swipeDelay)
			{
				Swipe();
			}
//			_rb.transform.rotation = Quaternion.LookRotation(rotations);
		}
	}

	void Swipe()
	{
		_swipeDelay = Time.time + SwipeRate;
		GameObject swipeClone = Instantiate(SwipeSound, transform.position, Quaternion.identity);
		Destroy(swipeClone, 1f);
	}
	
	void FixedUpdate()
	{
		Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		
		_rb.MovePosition(_rb.position + movement * Speed * Time.fixedDeltaTime);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Child"))
		{
//			GameObject annoyed = Instantiate(AnnoyedSound, transform.position, Quaternion.identity);
//			TakeDamage(other.gameObject.GetComponent<Child>().harm);
//			Destroy(annoyed, 1f);
		}

//		if (other.CompareTag("PowerUp"))
//		{
//			print("powerup: " + other.name);
//			if (other.name.StartsWith("GoldStar"))
//			{
//				GameObject goldSound = Instantiate(goldPowerUpSound, transform.position, Quaternion.identity);
//				Destroy(goldSound, 1f);
//				Destroy(other.gameObject);
//				StartCoroutine(PowerUp(0.05f, 3));
//			} else if (other.name.StartsWith("SilverBolt"))
//			{
//				GameObject silverSound = Instantiate(silverPowerUpSound, transform.position, Quaternion.identity);
//				Destroy(silverSound, 1f);				
//				Destroy(other.gameObject);
//				StartCoroutine(PowerUp(0.09f, 3));
//			}
//		}
	}

//	IEnumerator PowerUp(float fireRateIncrease, int forSeconds)
//	{
//		float originalRateOfFire = rateOfFire;
//		rateOfFire = fireRateIncrease;
//		yield return new WaitForSeconds(forSeconds);
//		rateOfFire = originalRateOfFire;
//	}

//	IEnumerator ShowDamage()
//	{
//		
//		sprite.color = Color.red;
//		yield return new WaitForSeconds(0.2f);
//		sprite.color = originalColor;
//	}
	
	void TakeDamage(int damage)
	{

//		StartCoroutine(ShowDamage());	
		
		Purrs += damage;

		if (Purrs > 3)
		{
			foreach (var purr in PurrMeter)
			{
				purr.gameObject.SetActive(false);
			}

			if (Purrs < 2)
			{
				NineLivesMeter[Purrs].SetActive(false);
			}
			else
			{
				foreach (var life in NineLivesMeter)
				{
					life.gameObject.SetActive(true);
				}
			}
		} 
		else
		{
			PurrMeter[Purrs].gameObject.SetActive(false);
		}
		
		if (Purrs >= 10)
		{
			Die();
		}
	}

	void Die()
	{
		Destroy(gameObject);
		GameManager.Instance.Lose();
	}
}








