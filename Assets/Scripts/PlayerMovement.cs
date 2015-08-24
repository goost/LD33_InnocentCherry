using System;
using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speedAdjust = 25f;
    [SerializeField] private float _overlapRadius = 0.001f;
    [SerializeField] private float _jumpForce = 275;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private LayerMask _groundMask;
    private  Rigidbody2D _rb;
    private  Animator _anim;
    private PlayerState _curState;
    private PlayerState _idle;
    private PlayerState _walk;
    private PlayerState _run;
    private PlayerState _jump;
    private bool _isGrounded;
    //private bool _isCutscene;
    public bool _isCutscene;

    // Use this for initialization
	void Awake ()
	{
	    _rb = GetComponent<Rigidbody2D>();
	    _anim = GetComponent<Animator>();
        _anim.SetFloat("speed_x", 1);
        _idle = new Idle(_anim);
        _run = new Run(_anim);
	    _walk = new Walk(_anim);
	    _jump = new Jump(_anim);
	    _curState = _idle;
	    _isCutscene = false;

	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
        if (_isCutscene) return;
	    var isRunning = Input.GetKey(KeyCode.LeftShift);
	    var movement = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

        _curState.ExitState();
	    if (movement != Vector2.zero && _isGrounded)
	    {    
	        _curState = isRunning ? _run : _walk;
	    }
	    else if (_isGrounded)
	    {
	        _curState = _idle;
           
	    }
        _curState.EnterState(movement.x);
        //_rb.MovePosition(_rb.position + movement *Time.deltaTime * _curState.Modifier() * _speedAdjust);
        _rb.velocity = new Vector2(movement.x *Time.deltaTime*_curState.Modifier() * _speedAdjust, _rb.velocity.y);
	}

    void Update()
    {
        GroundControl();
        if (_isCutscene) return;
        var jumped = Input.GetButtonDown("Jump");
        if (jumped && _isGrounded)
        {
            _rb.AddForce(new Vector2(0,_jumpForce));
            _curState.ExitState();
            _curState = _jump;
            _curState.EnterState(0);           
        }
    }

    private void GroundControl()
    {
        //Check a small area around the players foot, if there are any collision
        //if true, then we are grounded
        var groundedBefore = _isGrounded; //NOTE(goost) Dirty, dirty workaround
        _isGrounded = Physics2D.OverlapCircle(_groundChecker.position, _overlapRadius, _groundMask);
        _anim.SetBool("IsInAir", !_isGrounded);   
        if (!groundedBefore)
        {
            if (_isGrounded) Debug.Log("On Ground!");
            _curState.ExitState();
            _curState = _isGrounded ? _idle : _jump;
            _curState.EnterState(0);
        }
        
    }

    void StartCutscene()
    {
        _isCutscene = true;
        _rb.Sleep();
        _curState.ExitState();
        _curState = _idle;
        _curState.EnterState(0);
    }

    void EndCutscene()
    {
        _isCutscene = false;
        _rb.WakeUp();
    }

    private abstract class PlayerState
    {
        protected readonly Animator _anim;
        protected PlayerState(Animator anim)
        {
            _anim = anim;
        }

        public virtual void EnterState(float value){}
        public virtual void ExitState(){}
        public virtual float Modifier()
        {
            return 1;
        }

    }
    private abstract class XMove : PlayerState
    {
        protected XMove(Animator anim) : base(anim){}

        public override void EnterState(float value)
        {
            _anim.SetFloat("speed_x", value);
        }
    }

    private class Run : XMove
    {
        internal Run(Animator anim) : base(anim){}
        public override void EnterState(float value)
        {
            Debug.Log("Setting Running");
            _anim.SetBool("IsRunning", true);
            base.EnterState(value);
        }

        public override void ExitState()
        {
            _anim.SetBool("IsRunning", false);
        }

        public override float Modifier()
        {
            return 1.4f * base.Modifier();
        }
    }
    private class Walk : XMove
    {
        internal Walk(Animator anim) : base(anim){}
        public override void EnterState(float value)
        {
            _anim.SetBool("IsWalking", true);
            base.EnterState(value);
        }

        public override void ExitState()
        {
            _anim.SetBool("IsWalking", false);
        }
    }
    private class Idle : PlayerState
    {
        internal Idle(Animator anim) : base(anim){}
    }
    private class Jump : PlayerState
    {
        internal Jump(Animator anim) : base(anim) { }
        public override void EnterState(float value)
        {
            _anim.SetBool("IsInAir", true);
        }

        public override void ExitState()
        {
            _anim.SetBool("IsInAir", false);
            
        }

        public override float Modifier()
        {
            return 0.8f * base.Modifier();
        }
    }


}

