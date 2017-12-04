using UnityEngine;

public class KidBehavior : MonoBehaviour
{
    #region PROPS

    bool bInited = false;

    Vector3 startPos;

    public float speed;

    public float radar;

    public float wanderRadius;

    public SpriteRenderer view;

    public Rigidbody2D body;

    public float stoppingDistance;
    
    public float InitialSpeedIncrease;
    public float SpeedIncreaseRate;
    public float InitialRadarIncrease;
    public float RadarIncreaseRate;
    public float Radius = 5;


    GameObject cat;

    bool bCatIsVisible
    {
        get
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, cat.transform.position - transform.position, radar, 1 << LayerMask.NameToLayer("Obstacles"));
                if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
                {
                    return true;
                }

            return false;
        }
    }

    bool bCatIsPotentiallyVisible
    {
        get
        {
            return ((cat.transform.position - transform.position).magnitude < radar);
        }
    }

    Vector3 prevMovement;

    #endregion

    #region FSM PROPS

    public bool bDebugFSM;
    public string FSMInitState;

    FiniteStateMachine _FSM;
    protected FiniteStateMachine FSM
    {
        get
        {
            if (_FSM == null)
            {
                _FSM = new FiniteStateMachine(this);
                _FSM.bDebug = bDebugFSM;
            }

            return _FSM;
        }
    }

    public string State
    {
        get
        {
            return FSM.state;
        }
        set
        {
            FSM.state = value;
        }
    }
    public float StateTimer
    {
        get
        {
            return FSM.Timer;
        }
        set
        {
            FSM.Timer = value;
        }
    }
    public float StateTimerReal
    {
        get { return FSM.TimerReal; }
    }
    public float StateTimerRealInverse
    {
        get { return FSM.TimerRealInverse; }
    }
    public bool bStateTimerOut
    {
        get
        {
            return FSM.TimerOut;
        }
    }

    #endregion

    #region DEBUG

    private void OnDrawGizmos()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radar);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere((!bInited)?transform.position:startPos, wanderRadius);
    }

    #endregion

    #region BEHAVIOUR

    private void Awake()
    {
        startPos = transform.position;
        bInited = true;

        //inject the cat here
        cat = FindObjectOfType<PlayerController>().gameObject;

        State = "Idle";
        body = GetComponent<Rigidbody2D>();
        view = GetComponent<SpriteRenderer>();
        
        InvokeRepeating("IncreaseSpeed", InitialSpeedIncrease, SpeedIncreaseRate);
        InvokeRepeating("IncreaseRadar", InitialRadarIncrease, RadarIncreaseRate);

    }

    private void Update()
    {
        if (GameManager.Instance.GameStarted)
        {
            FSM.Update();
        }
    }

    #endregion

    #region PHYSICS

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            if(State != "Pet")
            {
                State = "Pet";
            }
        }
    }

    #endregion

    #region METHODS

    void IncreaseSpeed()
    {
        speed += 25;
    }

    void IncreaseRadar()
    {
        radar += 1;
    }
	
    void MoveTo(Vector3 target)
    {
        Move(Vector3.Normalize(target - transform.position) * speed * Time.deltaTime);
    }

    void Move(Vector3 velocity)
    {
        body.AddForce(velocity);

        //flip view
        view.flipX = (velocity.x == 0) ? view.flipX : (velocity.x < 0);

        prevMovement = velocity;
    }

    #endregion

    #region STATES

    #region Idle STATE

    public void Idle_Enter()
    {
        StateTimer = Random.Range(1.0f, 5.0f);
    }

    public void Idle_Update()
    {
        if (bCatIsVisible)
        {
            State = (cat.GetComponent<PlayerController>().IsProtected) ? "BackOff" : "Chase";
            return;
        }

        if (bStateTimerOut)
        {
            State = "Wander";
            return;
        }
    }

    public void Idle_Exit()
    {

    }
    #endregion


    #region Wander STATE
    Vector3 wanderTarget;
    public void Wander_Enter()
    {
        //get the wander target
        wanderTarget.x = startPos.x + wanderRadius * Random.Range(-1.0f, 1.0f);
        wanderTarget.y = startPos.y + wanderRadius * Random.Range(-1.0f, 1.0f);
    }

    public void Wander_Update()
    {
        if (bCatIsVisible)
        {
            State = (cat.GetComponent<PlayerController>().IsProtected)?"BackOff":"Chase";      // <<<<<<<<<<<<<<<<<<<<<<<<<<< insert your cat controller class here
            return;
        }

        //wander to target
        MoveTo(wanderTarget);

        //check if target is reached
        if((wanderTarget - transform.position).magnitude < stoppingDistance)
        {
            //quit to Idle
            State = "Idle";
        }
    }

    public void Wander_Exit()
    {

    }
    #endregion

    //insert chase sounds here on Enter
    #region Chase STATE
    public void Chase_Enter()
    {
        //get excited here

        KidSounds.Instance.PlayChase();
    }

    public void Chase_Update()
    {
        MoveTo(cat.transform.position);

        if(!bCatIsVisible)
        {
            //target lost

            //get upset here
            KidSounds.Instance.PlayGetaway();

            //quit to idle
            State = "Idle";
            return;
        }

        if (cat.GetComponent<PlayerController>().IsProtected)          // <<<<<<<<<<<<<<<<<<<<<<<<<<< insert your cat controller class here
        {
            //can't chase the Cat anymore :(
            State = "BackOff";
            return;
        }
    }

    public void Chase_Exit()
    {

    }
    #endregion


    #region BackOff STATE
    Vector3 backOffDir;
    public void BackOff_Enter()
    {
        //back off from the cat
        backOffDir = Vector3.Normalize(transform.position - cat.transform.position);
        
        KidSounds.Instance.PlayGetaway();
    }

    public void BackOff_Update()
    {
        Move(backOffDir * speed * Time.deltaTime);

        if (!bCatIsVisible)
        {
            //target lost
            State = "Idle";
            return;
        }

        if (!cat.GetComponent<PlayerController>().IsProtected)         // <<<<<<<<<<<<<<<<<<<<<<<<<<< insert your cat controller class here
        {
            //not protected anymore! :D
            State = "Chase";
            return;
        }
    }

    public void BackOff_Exit()
    {

    }
    #endregion

    //insert pet sounds here on Enter
    #region Pet STATE
    public void Pet_Enter()
    {
        //let the Cat know 
//        cat.SendMessage("OnBeingLoved");    //the cat should have a void OnBeingLoved() implemented

        //throw some hearts around, the kid is excited (animation?)

        //play sound

        //stays excited for some time
        //give Cat some time to get away
        StateTimer = 1.0f + Random.Range(0.0f, 1.0f);
    }

    public void Pet_Update()
    {
        if (bStateTimerOut)
        {
            State = "Idle"; 
        }
    }

    public void Pet_Exit()
    {

    }
    #endregion

    #endregion
}