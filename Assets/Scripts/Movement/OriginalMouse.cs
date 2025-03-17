using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.InputSystem;

public class OriginalMouse : NetworkBehaviour
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
        _childRigidbody = GetComponentInChildren<Rigidbody2D>();
    }

    // Update is called once per frame

    private void Update()
    {
        //if (!IsClient) return;

        //LeftClick();
        //RightClick();
    }
    void FixedUpdate()
    {
        //if (!IsOwner) return;
        //characterMovable.transform.position = new Vector3 (mouseWorldPosition.x, mouseWorldPosition.y, 0);
        _platformVelocity = _currentXVelocity;
        //var step = speed * Time.deltaTime;
        //transform.position = new Vector3(transform.position.x, currentY, transform.position.z);
        //characterMovable.transform.position = Vector3.MoveTowards(transform.position, mouseWorldPosition, step);
        _childRigidbody.linearVelocity = new Vector2(_currentXVelocity, _rigidbody.linearVelocity.y);
        //_platformVelocity = transform.position.x - mouseWorldPosition.x;
        //LeftClick();
        //RightClick();
        //Debug.Log(_rigidbody.velocity.x);
        //_platformVelocity = step;
        //Debug.Log(_platformVelocity);
    }

    void LeftClick()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!IsClient) return;
            //Debug.Log("Left click.");
            mousePosition = Mouse.current.position.ReadValue();
            mousePosition.z = Camera.main.nearClipPlane;
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mouseWorldPosition.z = 0;

            _platformVelocity = mouseWorldPosition.x - transform.position.x;

        }
    }

    void RightClick()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            //Debug.Log("Right click.");
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {

        xMovement = ctx.ReadValue<float>();

        float speedToAdd = xMovement * speed;

        Debug.Log(speedToAdd);
        //Debug.Log(speedToAdd);
        if (Mathf.Abs(xMovement) > 0.01f)
        {
            //Debug.Log("Move left");
            _currentXVelocity = speedToAdd;
        }
    }
}
