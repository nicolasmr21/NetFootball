using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Summary : MonoBehaviour
{
    public Text firstPlayerName;
    public Text secondPlayerName;
    public Text firstPlayerScore;
    public Text secondPlayerScore;
    public Text winnerName;
    public GlobalControl result;

    void Awake()
    {
        firstPlayerName = result.Instance.player1.GetComponent<Player>().Name;
        secondPlayerName = result.Instance.player2.GetComponent<Bot>().name;
        firstPlayerScore = result.Instance.ball.GetComponent<Ball>().score1;
        secondPlayerScore = result.Instance.ball.GetComponent<Ball>().score2;
        setWinner();
    }

    private void setWinner()
    {
        if (Int32.Parse(firstPlayerScore.text) > Int32.Parse(secondPlayerScore.text))
        {
            winnerName.text = firstPlayerName.text;
        }
        else if (Int32.Parse(firstPlayerScore) < Int32.Parse(secondPlayerScore))
        {
            winnerName.text = secondPlayerName.text;
        }
        else
        {
            winnerName.text = "Draw";
        }
    }
}
