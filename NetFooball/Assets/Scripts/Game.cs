using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    public GameObject player1;
    public GameObject player2;
    public GameObject ball;
    public Boolean initial;

    // Start is called before the first frame update
    void Start()
    {
        initial = false;
        Debug.Log("Nuevo");
        player1.GetComponent<Player>().Name = GameNetwork.ClientName;
    }


    internal void UpdatePlay(string dataReceive)
    {
        Debug.Log(dataReceive);

            if (dataReceive.Contains("Player1"))
            {
                player1.GetComponent<Player>().n = 2;
                Debug.Log("Player1" + GameNetwork.ClientName);
                Transform temp = player1.transform;
                
                player1.transform.position = player2.transform.position;
                player2.transform.position = new Vector3( player2.transform.position.x, player2.transform.position.y, float.Parse("-18.54659"));

            }

        try
        {
            string[] dataSplited = dataReceive.Split('|');

            if (dataSplited.Length>=14)
        {
            for (int i = 0; i < dataSplited.Length; i += 15)
            {

                    string playerName = dataSplited[i + 4];
                    float[] transform = new float[] { float.Parse(dataSplited[i + 5]), float.Parse(dataSplited[i + 6]), float.Parse(dataSplited[i + 7]),
                                                   float.Parse(dataSplited[i + 8]), float.Parse(dataSplited[i + 9]),float.Parse(dataSplited[i + 10]), float.Parse(dataSplited[i + 11])
                                                   ,float.Parse(dataSplited[i+12]),float.Parse(dataSplited[i+13]),float.Parse(dataSplited[i+14]) };
                    //
                    Debug.Log(playerName);

                    
                    if (playerName != player1.GetComponent<Player>().Name)
                    {

                        player2.transform.position =new Vector3(transform[0], transform[1], transform[2]) ;
                        player2.transform.rotation = new Quaternion(transform[3], transform[4], transform[5], transform[6]);
                        //ball.transform.Translate(new Vector3(transform[7], transform[8], transform[9]));

                    }

                }


        }

        }


                catch (Exception e)
        {


        }
    }
}
