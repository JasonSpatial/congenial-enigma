using UnityEngine;

public class KidController : MonoBehaviour
{
	public int HarmAmount;
	public GameObject ScaredSound;
	public bool IsPatrolling = true;
	public float Speed = 5.5f;
	public float KidRadar = 5;

	private Transform _target;
	private Rigidbody2D _rb;
	private Vector3 _origin;
	private Vector2 _direction;
	
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
					IsPatrolling = false;
					var impulse = (_direction).normalized * Time.deltaTime;
					_rb.AddForce(impulse * Speed);
				}
				else
				{
					GoHome();
				}
			}
			else
			{
				// return to patrol area
				GoHome();
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
	}

	void GoHome()
	{
		_direction = _origin - transform.position;
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
