using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : NetworkBehaviour
{
    [SerializeField]
    SoundFXManager sfxManager;
    [SerializeField]
    AudioClip jumpSound;
    [SerializeField]
    MouseDetection player2;
    Camera initialCamera;
    // track inputs
    private float xMovement;
    private float _currentXVelocity = 0;
    private float _currentYVelocity = 0;
    // serialized fields
    [SerializeField] private float speed = 10f;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask obstacles;
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float doubleJumpForce = 2.0f;
    [SerializeField] private float maxSpeed = 100.0f;
    [SerializeField] private float acceleration = 10.0f;
    [SerializeField] private float deceleration = 5.0f;
    [SerializeField] private float groundFriction = 0.3f;

    [SerializeField] private float gravityForce = 9.81f;
    
    public ConstantForce2D gravity;

    // components
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private bool grounded;

    // for location
    private float startX;
    private float startY;
    public bool touchingOtherPlayer = false;



    
    // Start is called before the first frame update
    private void Awake()
    {
       _rigidbody = GetComponent<Rigidbody2D>();
       _collider = GetComponent<Collider2D>();
        startX = this.transform.position.x;
        startY = this.transform.position.y;

       // initialCamera = Camera.current;
       // initialCamera.enabled = false;
        sfxManager = FindFirstObjectByType<SoundFXManager>();
        GravityMath();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsOwner) return;
        float multiplier = 3f;
        // update velocity
        //Debug.Log(touchingOtherPlayer);
        if (touchingOtherPlayer)
        {
            if (player2._platformVelocity > 0.5f)
            {
                _currentXVelocity = player2._platformVelocity;
            }
            else if (player2._platformVelocity < -0.5f)
            {
                _currentXVelocity = player2._platformVelocity;
            }
            
        }
        _rigidbody.linearVelocity = new Vector2(_currentXVelocity, _rigidbody.linearVelocity.y);


        KinematicCalls();
    }

    private void KinematicCalls ()
    {
        // ground friction
        GroundFrictionMath();

        IsGrounded();
        // gravity
        //GravityMath();
    }
    public void GroundFrictionMath()
    {
        if(Mathf.Abs(xMovement) < 0.01f && _currentXVelocity > 0.3f)
        {
            _currentXVelocity *= 0.5f;
        }
        else if(Mathf.Abs(xMovement) < 0.01f && _currentXVelocity < 0.3f)
        {
            _currentXVelocity = 0;
        }

    }

    public void GravityMath()
    {
        gravity = gameObject.AddComponent<ConstantForce2D>();
        gravity.force = new Vector3(0.0f, -1 * gravityForce, 0.0f);
    }
    public void OnMove(InputAction.CallbackContext ctx)
    {
        //Debug.Log("Call move 2.");
        xMovement = ctx.ReadValue<float>();
        
        float speedToAdd = xMovement * speed;
        //Debug.Log(speedToAdd);
        if (Mathf.Abs(xMovement) > 0.01f)
        {
            //Debug.Log("Move left");
            _currentXVelocity = speedToAdd;
        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        Vector3 jump = new Vector3(0, 1, 0);

        if (grounded && ctx.performed)
        {
            //Debug.Log("Can jump");
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, 0);
            _rigidbody.AddForce(jump * jumpForce, ForceMode2D.Impulse);
            sfxManager.PlayClip(jumpSound, transform, 0.5f);
        }
        
    }

    public void IsGrounded()
    {
        grounded = (Physics2D.Raycast((new Vector2(_rigidbody.transform.position.x, _rigidbody.transform.position.y + 1.25f)), Vector3.down, 2f, 1 << LayerMask.NameToLayer("Ground")));
        //Debug.Log(Time.time);
        //grounded = (Physics2D.BoxCast((new Vector2(_rigidbody.transform.position.x, _rigidbody.transform.position.y + 1.25f)), Vector3.down));
        Debug.DrawRay((new Vector3(_rigidbody.transform.position.x, _rigidbody.transform.position.y + 0.5f, _rigidbody.transform.position.z)), Vector3.down, Color.green, 5);
        

    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (player2 == null)
        {
            player2 = FindFirstObjectByType<MouseDetection>();
        }
        if (other.gameObject.CompareTag("Player2"))
        {
            touchingOtherPlayer = true;
        }
        else
        {
            touchingOtherPlayer = false;
        }
    }

    void OnCollisionExit2D (Collision2D col)
    {
        if (col.gameObject.CompareTag("Player2"))
        {
            touchingOtherPlayer = true;
        }
        else
        {
            touchingOtherPlayer = false;
        }
    }

}
