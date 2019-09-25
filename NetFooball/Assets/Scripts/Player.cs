using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public string Name;
    public int n;
    public Transform aimTarget; // the target where we aim to land the ball
    public Transform aimTarget2; // the target where we aim to land the ball
    float speed = 3; // move speed
    float force = 6; // ball impact force
    private Rigidbody selfRigidbody;
    public Vector3 initialPos; // ball's initial position
    public int score;

    bool hitting; // boolean to know if we are hitting the ball or not 
    bool isInPosition; //Boolean to know if the player is in position to hit

    public Transform ball; // the ball 
    Animator animator;

    Vector3 aimTargetInitialPosition; // initial position of the aiming gameObject which is the center of the opposite court
    Vector3 aimTarget2InitialPosition; // initial position of the aiming gameObject which is the center of the opposite court
    



    ShotManager shotManager; // reference to the shotmanager component
    Shot currentShot; // the current shot we are playing to acces it's attributes

    private void Start()
    {
        
        score = 0;
        initialPos = transform.position;
        n = 1;
        selfRigidbody = gameObject.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // referennce out animator
        aimTargetInitialPosition = aimTarget.position; // initialise the aim position to the center( where we placed it in the editor )
        aimTarget2InitialPosition = aimTarget2.position; // initialise the aim position to the center( where we placed it in the editor )
        shotManager = GetComponent<ShotManager>(); // accesing our shot manager component 
        currentShot = shotManager.topSpin; // defaulting our current shot as topspin
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal") ; // get the horizontal axis of the keyboard
        float v = Input.GetAxis("Vertical") ; // get the vertical axis of the keyboard

        if (Input.GetKeyDown(KeyCode.F))
        {
            hitting = true; // we are trying to hit the ball and aim where to make it land
            currentShot = shotManager.topSpin; // set our current shot to top spin
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            hitting = false; // we let go of the key so we are not hitting anymore and this 
        }                    // is used to alternate between moving the aim target and ourself

        if (Input.GetKeyDown(KeyCode.E))
        {
            hitting = true; // we are trying to hit the ball and aim where to make it land
            currentShot = shotManager.flat; // set our current shot to top spin
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            hitting = false;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 3;
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 6;
        }

        if (hitting)  // if we are trying to hit the ball
        {
            aimTarget.Translate(new Vector3(h, 0, 0) * speed * 2 * Time.deltaTime); //translate the aiming gameObject on the court horizontallly
        }


        if ((h != 0 || v != 0) ) // if we want to move and we are not hitting the ball
        {
            

             transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime); // move on the court

            float x = transform.position.x;
            float y = transform.position.y;
            float z = transform.position.z;

            if (x < -7 ) {
                transform.position = new Vector3(-7f,y,z);
            }
            if (x > 7.5)
            {
                transform.position = new Vector3(7.5f, y, z);
            }
            if (z > 9)
            {
                transform.position = new Vector3(x, y, 9f);
            }
            if (z < -13.5)
            {
                transform.position = new Vector3(x, y, -13.5f);
            }


        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            selfRigidbody.AddForce(0, 10, 0, ForceMode.Impulse);
        }

        if(n==1)
            score = ball.GetComponent<Ball>().score1;
        else
            score = ball.GetComponent<Ball>().score2;

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball")) // if we collide with the ball 
        {
            Vector3 dir;

            if (n == 1)
                dir = aimTarget.position - transform.position; // get the direction to where we want to send the ball

            else
                dir = aimTarget2.position - transform.position; // get the direction to where we want to send the ball

            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);
            //add force to the ball plus some upward force according to the shot being played

            Vector3 ballDir;
            ballDir = ball.position - transform.position; // get the direction of the ball compared to us to know if it is



            if (ballDir.x >= 0)                                   // on out right or left side 
            {
                animator.Play("forehand");                        // play a forhand animation if the ball is on our right
            }
            else                                                  // otherwise play a backhand animation 
            {
                animator.Play("backhand");
            }

            if (n == 1)
            aimTarget.position = aimTargetInitialPosition; // reset the position of the aiming gameObject to it's original position ( center)

            else
            aimTarget2.position = aimTargetInitialPosition; // reset the position of the aiming gameObject to it's original position ( center)

        }
    }


    
}