using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public float time = 60.0f;
    public int part;
    public int n;
    public Text waiting;
    public int tipePlayer = 0;//jugador 1 o 2 
    public static bool fin = false;



    // Start is called before the first frame update
    void Start()
    {
        part = 1;
        initial = false;
        Debug.Log("Nuevo");

        player1.GetComponent<Player>().Name = GameNetwork.ClientName;

        waiting.text = "";
    }

    private void Update()
    {

        if (time <= 0 && part == 1)
        {
            time = 60.0f;
            part++;

            ball.transform.position = ball.GetComponent<Ball>().initialPos;
            if(tipePlayer == 0) {
                player1.transform.position = player1.GetComponent<Player>().initialPos;
                player2.transform.position = player2.GetComponent<Bot>().initialPos;
            } else {
                player2.transform.position = player1.GetComponent<Player>().initialPos;
                player1.transform.position = player2.GetComponent<Bot>().initialPos;
            }

        }
        else if(time <= 0 && part <= 2) {
            fin = true;
            saveData();
            SceneManager.LoadScene(3);
            //GameNetwork.clientSocket.Close();
        }
        else {
            time = time - 1 * Time.deltaTime;
            timeText.text = time.ToString("f0")+"";
        }
    }


    internal void UpdatePlay(string dataReceive)
    {
        if(fin == false) {
        lock (this);
        Debug.Log(dataReceive);

        if(dataReceive.Contains("Player1")) {
            tipePlayer = 0;
            player1.GetComponent<Player>().n = 1;
            Debug.Log("Player1" + GameNetwork.ClientName);
            Transform temp = player1.transform;
            time = 60.0f;

            player1.transform.position = player1.GetComponent<Player>().initialPos;
            player2.transform.position = player2.GetComponent<Bot>().initialPos;
        }

        else if (dataReceive.Contains("Player2"))
        {
            tipePlayer = 1;
            player1.GetComponent<Player>().n = 2;
            Debug.Log("Player2" + GameNetwork.ClientName);
            Transform temp = player1.transform;
            time = 60.0f;

            player1.transform.position = player2.GetComponent<Bot>().initialPos;
            player2.transform.position = player1.GetComponent<Player>().initialPos;
        }
        else 
        {
            if (time <= 60.0f && time > 0)
            {
                try
                {
                    string[] dataSplited = dataReceive.Split('|');
                    for(int i = 0; i < dataSplited.Length; i+=13) {


                            string playerName = dataSplited[0 + i];
                            float[] transform = new float[] {float.Parse(dataSplited[1 + i]), float.Parse(dataSplited[2 + i]), float.Parse(dataSplited[3 + i])
                                                           ,float.Parse(dataSplited[4 + i]) , float.Parse(dataSplited[5 + i]),float.Parse(dataSplited[6 + i]), float.Parse(dataSplited[7 + i])
                                                           ,float.Parse(dataSplited[8 + i]) ,float.Parse(dataSplited[9 + i]), float.Parse(dataSplited[10 + i])
                                                           ,float.Parse(dataSplited[11 + i]) ,float.Parse(dataSplited[12 + i])};
                            //
                            Debug.Log(playerName);
                
                            if (playerName != player1.GetComponent<Player>().Name)
                            {
                                player2.transform.position = new Vector3(transform[0], transform[1], transform[2]);
                                player2.transform.rotation = new Quaternion(transform[3], transform[4], transform[5], transform[6]);

                                float x = ball.transform.position.x;
                                float y = ball.transform.position.y;
                                float z = ball.transform.position.z;

                                if (x < transform[7] + 0.5 && x > transform[7] - 0.5 && y < transform[8] + 0.5 && y > transform[8] - 0.5
                                    && z < transform[9] + 0.5 && z > transform[9] - 0.5)
                                {
                                    ball.GetComponent<Rigidbody>().MovePosition(new Vector3(transform[7], transform[8], transform[9]));
                                }

                                ball.GetComponent<Ball>().score2 = int.Parse(transform[10].ToString());
                                player2.GetComponent<Bot>().name = playerName;

                            }
                            else
                            {


                            }
                   }

                }

            catch (Exception e)
            {


            }
            }
        }
       }
    }

    private string setWinner()
    {

        int firstPlayerScore = ball.GetComponent<Ball>().score1;
        int secondPlayerScore = ball.GetComponent<Ball>().score2;

        if (firstPlayerScore > secondPlayerScore)
        {
            return player1.GetComponent<Player>().Name;
        }
        else if (firstPlayerScore < secondPlayerScore)
        {
            return player2.GetComponent<Bot>().name;
        }
        else
        {
            return "Draw";
        }
    }

    void saveData()
    {
        string winner = setWinner();
        string directory = Directory.GetParent(Environment.CurrentDirectory).ToString();
        string[] gameData = { player1.GetComponent<Player>().Name, player2.GetComponent<Bot>().name, ball.GetComponent<Ball>().score1.ToString(), ball.GetComponent<Ball>().score2.ToString(), winner };
        using (StreamWriter outputFile = new StreamWriter(Path.Combine(directory, @"GameData", "summary.txt")))
        {
            foreach (string line in gameData)
            {
                outputFile.WriteLine(line);
            }
        }
        SceneManager.LoadScene(3);
    }
}
