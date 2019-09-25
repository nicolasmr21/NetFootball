using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Vector3 initialPos; // ball's initial position

    public int score1;
    public int score2;

    private void Start()
    {
        initialPos = transform.position; // default it to where we first place it in the scene
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall")) // if the ball hits a wall
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero; // reset it's velocity to 0 so it doesn't move anymore
            transform.position = initialPos; // reset it's position 
            score1 = score1 + 1;
        }

        else if (collision.transform.CompareTag("Wall2")) // if the ball hits a wall
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero; // reset it's velocity to 0 so it doesn't move anymore
            transform.position = initialPos; // reset it's position 
            score2 = score2 + 1;
        }
    }

    public void Update(){
        GameObject.Find("Score1").GetComponent<TextMesh>().text = score1 + "";
        GameObject.Find("Score2").GetComponent<TextMesh>().text = score2 + "";
    }

}