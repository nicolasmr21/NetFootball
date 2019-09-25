using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    public GameObject player1;
    public GameObject player2;
    public GameObject ball;
    public Boolean initial;
    public Text timeText;
    float time = 60.0f;
    int part;
    int n;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad (transform.gameObject);
        part = 1;
        initial = false;
        Debug.Log("Nuevo");
        player1.GetComponent<Player>().Name = GameNetwork.ClientName;
    }

    private void Update()
    {
        time = time - 1 * Time.deltaTime;
        timeText.text = time.ToString("f0")+"";

        if (time < 0 && part == 1)
        {
            time = 60.0f;
            part++;
            player1.transform.position = player1.GetComponent<Player>().initialPos;
            player2.transform.position = player2.GetComponent<Bot>().initialPos;

        }
        else if(time <0 && part==2) {
            SceneManager.LoadScene(3);
        }
    }


    internal void UpdatePlay(string dataReceive)
    {
        Debug.Log(dataReceive);

            if (dataReceive.Contains("Player2"))
            {
                player1.GetComponent<Player>().n = 2;
                Debug.Log("Player2" + GameNetwork.ClientName);
                Transform temp = player1.transform;
                
                player1.transform.position = player2.transform.position;
                player2.transform.position = new Vector3( player2.transform.position.x, player2.transform.position.y, float.Parse("-18.54659"));
                

            }

        try
        {
            string[] dataSplited = dataReceive.Split('|');

            if (dataSplited.Length>=15)
        {
                for (int i = 0; i < dataSplited.Length; i += 16)
                {

                    string playerName = dataSplited[i + 4];
                    float[] transform = new float[] { float.Parse(dataSplited[i + 5]), float.Parse(dataSplited[i + 6]), float.Parse(dataSplited[i + 7]),
                                                   float.Parse(dataSplited[i + 8]), float.Parse(dataSplited[i + 9]),float.Parse(dataSplited[i + 10]), float.Parse(dataSplited[i + 11])
                                                   ,float.Parse(dataSplited[i+12]),float.Parse(dataSplited[i+13]), float.Parse(dataSplited[i+14])
                    ,float.Parse(dataSplited[i+15]) };
                    //
                    Debug.Log(playerName);

                  
                    if (playerName != player1.GetComponent<Player>().Name)
                    {
                        player2.transform.position = new Vector3(transform[0], transform[1], transform[2]);
                        player2.transform.rotation = new Quaternion(transform[3], transform[4], transform[5], transform[6]);
                        ball.GetComponent<Rigidbody>().MovePosition(new Vector3(transform[7], transform[8], transform[9]));
                        ball.GetComponent<Ball>().score2 = int.Parse(transform[10].ToString());
                        player2.GetComponent<Bot>().name = playerName;

                    }
                    else
                    {

                        ball.GetComponent<Ball>().score1 = int.Parse(transform[10].ToString());

                    }

                   
                }


        }
        }


        catch (Exception e)
        {


        }
    }
}
