using UnityEngine;

public class KidController : MonoBehaviour
{
	public int HarmAmount;
	public GameObject ScaredSound;
	public bool IsPatrolling = true;
	public float Speed = 5.5f;
	public float KidRadar = 5;
	public float Radius = 5;
	public float InitialSpeedIncrease;
	public float SpeedIncreaseRate;
	public float InitialRadarIncrease;
	public float RadarIncreaseRate;
	
	private bool _clipPlayed;
	private Transform _target;
	private Rigidbody2D _rb;
	private Vector3 _origin;
	private Vector3 _wanderPoint;
	private Vector2 _direction;
	private bool _targeted;
	private SpriteRenderer _sprite;
	
	void OnDrawGizmos()
	{
		// radar gizmo
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, KidRadar);
		
		// wander gizmo
		if (_origin != Vector3.zero)
		{
			Gizmos.color = Color.black;
			Gizmos.DrawWireSphere(_origin, Radius);
		}
		
	}
	
	private void FixedUpdate()
	{
		if (_target != null)
		{
			_direction = _target.position - transform.position;
			RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction);

			if (hit.collider != null)
			{
				if (hit.collider.CompareTag("Player") && hit.distance <= KidRadar &&
				    !hit.collider.gameObject.GetComponent<PlayerController>().IsProtected)
				{
					_targeted = true;
					_sprite.flipX = ((_direction).normalized.x == 0) ? _sprite.flipX : ((_direction).normalized.x < 0);
					var impulse = (_direction).normalized * Time.deltaTime;
					_rb.AddForce(impulse * Speed);
					if (!_clipPlayed)
					{
//						ClipPlayer.Instance.ClipToPlay = RandomChaseSound();
//						ClipPlayer.Instance.PlayClip();
						_clipPlayed = true;
					}
				}
				else
				{
					if (_targeted)
					{
						SetWanderPoint();
//						ClipPlayer.Instance.ClipToPlay = RandomGetAwaySound();
//						ClipPlayer.Instance.PlayClip();
						_targeted = false;
					}

					GoAway();
					_clipPlayed = false;
				}
			}
			else
			{
				GoAway();
				_clipPlayed = false;
			}
		}
	}

	
	void Awake()
	{
		_origin = transform.position;
	}
	
	void Start ()
	{
		_rb = GetComponent<Rigidbody2D>();
		_target = GameObject.FindGameObjectWithTag("Player").transform;
		_sprite = GetComponent<SpriteRenderer>();
		InvokeRepeating("IncreaseSpeed", InitialSpeedIncrease, SpeedIncreaseRate);
		InvokeRepeating("IncreaseRadar", InitialRadarIncrease, RadarIncreaseRate);
	}

	void IncreaseSpeed()
	{
		Speed += 25;
	}

	void IncreaseRadar()
	{
		KidRadar += 1;
	}
	
	void SetWanderPoint()
	{
		print("origin: " + _origin);
		var newX = _origin.x + Radius * Random.Range(-1.0f, 1.0f);
		var newY = _origin.y + Radius * Random.Range(-1.0f, 1.0f);
		_wanderPoint.x = newX;
		_wanderPoint.y = newY;
		print("new origin: " + _origin);
	}

	void GoAway()
	{
		var destination = _wanderPoint == Vector3.zero ? _origin : _wanderPoint;
		_direction = destination - transform.position;
		var impulse = (_direction).normalized * Time.deltaTime;
		_rb.AddForce(impulse * Speed);
	}


	
	void Update()
	{
		if (_target != null)
		{

//			var impulse = (_target.position - transform.position).normalized * Time.deltaTime;
//			_rb.AddForce(impulse * Speed);
//			
//			transform.Translate((_target.position - transform.position).normalized * Time.deltaTime);

//			Vector3 randomization = new Vector2(Random.Range(0f, 0.2f), Random.Range(0f, 0.2f));
//			transform.rotation = 
//				Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_target.position - transform.position + randomization), RotationSpeed * Time.fixedDeltaTime);

//			transform.Translate(_target.position - transform.position + randomization * Time.fixedDeltaTime * Speed);
//			transform.position += transform.forward * Speed * Time.deltaTime;
		}
		
	}

//	public void TakeDamage(int damage = 1)
//	{
//		life -= damage;
//		
//		GameObject hitClone = Instantiate(hitSound, transform.position, Quaternion.identity);
//		Destroy(hitClone, 1f);
//
//		if (life <= 0)
//		{
//			if (Random.Range(1, 15) == powerUpTarget)
//			{
//				Instantiate(powerUp, transform.position, Quaternion.identity);
//			}
//			
//			GameManager.Instance.IncreaseScore();
//			Destroy(gameObject);
//		}
//		else if(life == 1)
//		{
//			speed *= 1.2f;
//		}
//	}
}
