using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseDetection : NetworkBehaviour
{

    // parameters
    [SerializeField]
    Transform test;
    private float xMovement;
    public float _currentXVelocity;
    private float targetX;
    private float targetY;

    private float currentX;
    private float currentY;

    [SerializeField]
    bool runNetwork = false;
    Vector3 mousePosition;
    Vector3 mouseWorldPosition;

    private Rigidbody2D _childRigidbody;

    [SerializeField]
    GameObject characterMovable;

    [SerializeField]
    float speed = 10.0f;

    public float _platformVelocity;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    // Start is called before the first frame update
    void Start()
    {
        currentX = this.transform.position.x;
        currentY = this.transform.position.y;
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        //_childRigidbody = GetComponentInChildren<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (!IsOwner) return;
        //
        _platformVelocity = _currentXVelocity;

        _rigidbody.linearVelocity = new Vector2(_currentXVelocity, _rigidbody.linearVelocity.y);
        //_rigidbody.position = new Vector2(_currentXVelocity, _childRigidbody.position.y);
    }


    public void MovePlatform(InputAction.CallbackContext ctx)
    {
        
        xMovement = ctx.ReadValue<float>();

        float speedToAdd = xMovement * speed;

        Debug.Log(speedToAdd);
        //Debug.Log(speedToAdd);
        if (Mathf.Abs(xMovement) > 0.01f)
        {
            _currentXVelocity = speedToAdd;
        }
        else if (ctx.canceled)
        {
            _currentXVelocity = 0;
        }
    }
}
