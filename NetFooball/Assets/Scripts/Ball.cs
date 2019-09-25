using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    public Vector3 initialPos; // ball's initial position

    public int score1;
    public int score2;

    public Text c1;
    public Text c2;


    private void Start()
    {
        initialPos = transform.position; // default it to where we first place it in the scene
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall2")) // if the ball hits a wall
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero; // reset it's velocity to 0 so it doesn't move anymore
            transform.position = initialPos; // reset it's position 
            score2 = score2 + 1;
        }

        else if (collision.transform.CompareTag("Wall")) // if the ball hits a wall
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero; // reset it's velocity to 0 so it doesn't move anymore
            transform.position = initialPos; // reset it's position 
            

            score1 = score1 + 1;

        }
    }

    private void Update()
    {
        c1.text = score1 + "";

        c2.text = score2 + "";

    }
}