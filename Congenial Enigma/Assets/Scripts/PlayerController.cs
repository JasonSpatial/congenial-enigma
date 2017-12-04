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
	public AudioClip PurrClip;
	public GameObject[] AnnoyedClips;
	
	private Rigidbody2D _rb;
	private float _swipeDelay;
	private Color _originalColor;
	private SpriteRenderer _sprite;
	private Animator _anim;
	private int _startingLife;
	private float _currentDirection;
	private AudioSource _audioSource;
	private AdultController _currentAdult;
	
	private void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
		_sprite = GetComponentInChildren<SpriteRenderer>();
		_originalColor = _sprite.color;
		_anim = GetComponent<Animator>();
		_audioSource = GetComponent<AudioSource>();
	}

	void Update () {
		if (GameManager.Instance.GameStarted)
		{

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
							PlayPurr(adult);
							break;	
						}
					
					}
				}
			}		
		}

	}

	void PlayPurr(AdultController adult)
	{
		if(!_audioSource.isPlaying && _currentAdult != adult)
		{
			_currentAdult = adult;
			_audioSource.clip = PurrClip;
			_audioSource.volume = 0.5f;
			_audioSource.Play();
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
		if (GameManager.Instance.GameStarted)
		{
			Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			_sprite.flipX = (movement.x == 0) ? _sprite.flipX : (movement.x < 0);
		
			_rb.MovePosition(_rb.position + movement * Speed * Time.fixedDeltaTime);
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Child") && GameManager.Instance.GameStarted)
		{
			_anim.SetTrigger("Hurt");
			var prefabAnnoyedSound = AnnoyedClips[Random.Range(0, AnnoyedClips.Length)];
			GameObject annoyed = Instantiate(prefabAnnoyedSound, transform.position, Quaternion.identity);
			Destroy(annoyed, 3);
			TakeDamage(1);
		}
	}

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
