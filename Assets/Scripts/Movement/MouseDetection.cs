using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseDetection : MonoBehaviour
{

    // parameters
    [SerializeField]
    Transform test;

    private float targetX;
    private float targetY;

    private float currentX;
    private float currentY;

    Vector3 mousePosition;
    Vector3 mouseWorldPosition;

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
    }

    // Update is called once per frame

    private void Update()
    {
        LeftClick();
        RightClick();
    }
    void FixedUpdate()
    {

        //characterMovable.transform.position = new Vector3 (mouseWorldPosition.x, mouseWorldPosition.y, 0);

        var step = speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, currentY, transform.position.z);
        characterMovable.transform.position = Vector3.MoveTowards(transform.position, mouseWorldPosition, step);

        _platformVelocity = transform.position.x - mouseWorldPosition.x;
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
}
