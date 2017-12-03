 using System.Linq;
 using UnityEngine;
 using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public int Purrs = 0;
	public float Speed = 1.0f;
	public float SwipeRate = 0.5f;
	public GameObject PurrSound;
	public GameObject SwipeSound;
	public GameObject AnnoyedSound;
	public GameObject[] HealthMeter;
	public GameObject[] Lives;
	public Texture FullHeart;
	public Texture HalfHeart;
	public Texture EmptyHeart;
	public bool IsProtected = false;
	
	private Rigidbody2D _rb;
	private float _swipeDelay;
	private Color _originalColor;
	private SpriteRenderer _sprite;
	private Animator _anim;
	private int _startingLife;
	private float _currentDirection;
	
	private void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
		_sprite = GetComponentInChildren<SpriteRenderer>();
		_originalColor = _sprite.color;
		_anim = GetComponent<Animator>();
	}


	void Update () {

		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 2f, Vector2.zero, 2f);
		IsProtected = false;
		
		foreach (var hit in hits)
		{
			if (hit.collider != null)
			{
				if(hit.collider.CompareTag("Adult"))
				{
					var adult = hit.collider.gameObject.GetComponent<AdultController>();
					if (!adult.Leaving)
					{
						adult.StartTimer();
						IsProtected = true;
						adult.PlayProtectedClip();
						break;	
					}
					
				}
			}
		}
		
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
		}
	}

	void Swipe()
	{
		_anim.SetTrigger("Attack");
		_swipeDelay = Time.time + SwipeRate;
		GameObject swipeClone = Instantiate(SwipeSound, transform.position, Quaternion.identity);
		Destroy(swipeClone, 1f);
	}
	
	void FixedUpdate()
	{
		Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		_sprite.flipX = (movement.x == 0) ? _sprite.flipX : (movement.x < 0);
		
		_rb.MovePosition(_rb.position + movement * Speed * Time.fixedDeltaTime);

		
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Child"))
		{

			_anim.SetTrigger("Hurt");
//			GameObject annoyed = Instantiate(AnnoyedSound, transform.position, Quaternion.identity);
			TakeDamage(1);
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
		var heartIndex = Mathf.CeilToInt(Purrs / 2);
		Purrs += damage;
		var currentSprite = HealthMeter[heartIndex].GetComponent<RawImage>();

		if (currentSprite.texture == EmptyHeart)
		{
			currentSprite.texture = HalfHeart;
		}
		else
		{
			currentSprite.texture = FullHeart;
		}
		
		if (Purrs >= 10)
		{
			Die();
		}
	}

	void Die()
	{
		GameManager.Instance.Lose();
	}
}








