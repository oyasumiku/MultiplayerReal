using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class BallCollision : NetworkBehaviour
{
    SoundFXManager sfxManager;
    [SerializeField]
    AudioClip playerBump;
    [SerializeField]
    AudioClip leftDrop;
    [SerializeField]
    AudioClip rightDrop;
    [SerializeField]
    AudioClip prepareSound;
    [SerializeField]
    AudioClip launchSound;
    [SerializeField]
    AudioClip wallBounceSound;
    [SerializeField]
    public float bounceForce = 10.0f;
    [SerializeField]
    public float playerForce = 20.0f;

    

    [SerializeField]
    public float launchX = 6.0f;
    [SerializeField]
    public float launchY = 10.0f;

    private bool floatBall = false;
    [SerializeField]
    Vector2 startPosition = new Vector2(3.5f, 10);
    private Rigidbody2D _rigidbody;

    private bool firstRun = true;
    [SerializeField]
    public float rotationForce = 30.0f;
    public ScoreUpdate _scoreUpdate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get necessary components
        _rigidbody = GetComponent<Rigidbody2D>();

        // find any necessary objects
        sfxManager = FindFirstObjectByType<SoundFXManager>();

        _scoreUpdate = GetComponent<ScoreUpdate>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // check how many players have joined
        bool lessThanTwoPlayers = GameObject.FindGameObjectsWithTag("Player1").Length < 2;
        
        // check if all players have joined and launch it the first time
        if (firstRun && !lessThanTwoPlayers)
        {
            StartCoroutine(launchBall(true));
            firstRun = false;
        }

        // hold the ball in place if
        // 1) someone scored
        // 2) there are less than 2 players
        if (floatBall || lessThanTwoPlayers)
        {
            transform.position = startPosition;
            _rigidbody.linearVelocityY = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        // if ball collides with players, add x and y velocity to it
        GameObject temp = collision.gameObject;
        if (temp.CompareTag("Player1"))
        {
            //Debug.Log("hit player");
            int direction = 0;

            // determine direction ball should be launched in
            if (_rigidbody.linearVelocityX > 0)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }
            Vector2 velocity = new Vector2((_rigidbody.linearVelocityX + playerForce) * direction, bounceForce);
            _rigidbody.linearVelocity = velocity;

            sfxManager.PlayClip(playerBump, transform, 0.25f);
        }

        // check if floor
        else if (temp.CompareTag("floor1"))
        {
            sfxManager.PlayClip(leftDrop, transform, 0.3f);
            //Debug.Log("hit floor 1");

            // increment the scoreboard
            _scoreUpdate.incrementP2score();

            // move ball to start and launch it
            bool player1loss = true;
            StartCoroutine(launchBall(player1loss));
        }
        else if (temp.CompareTag("floor2"))
        {
            sfxManager.PlayClip(rightDrop, transform, 0.3f);

            
            // increment the scoreboard
            _scoreUpdate.incrementP1score();

            // move ball to start and launch it
            bool player1loss = false;
            StartCoroutine(launchBall(player1loss));
        }
        //play audio on a wall bounce
        else if (temp.CompareTag("wall"))
        {
            sfxManager.PlayClip(wallBounceSound, transform, 0.25f);

            // give the ball a slight boost on hitting a wall
            _rigidbody.linearVelocity += new Vector2(0, 6.0f);
        }
    }

    // respawn the ball function 
    IEnumerator launchBall(bool player1)
    {
        floatBall = true;

        StartCoroutine(playLaunchAudio());
        //Wait for 2 seconds
        yield return new WaitForSeconds(2.3f);

        floatBall = false;

        if (player1)
        {
            // set rotation of the ball 
            _rigidbody.angularVelocity = 0;
            _rigidbody.AddTorque(-1 * rotationForce);

            // launch ball to left
            transform.position = startPosition;
            _rigidbody.linearVelocity = new Vector2(-1 * launchX, launchY);
        }
        else
        {
            // set rotation of ball
            _rigidbody.angularVelocity = 0;
            _rigidbody.AddTorque(rotationForce);

            // launch ball to the right
            _rigidbody.linearVelocity = new Vector2(launchX, launchY);
        }
        

    }

    IEnumerator playLaunchAudio()
    {
        sfxManager.PlayClip(prepareSound, transform, 0.25f);

        yield return new WaitForSeconds(1);

        sfxManager.PlayClip(prepareSound, transform, 0.25f);

        yield return new WaitForSeconds(1);

        sfxManager.PlayClip(launchSound, transform, 0.25f);
    }
}
