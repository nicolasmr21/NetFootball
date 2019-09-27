using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Summary : MonoBehaviour
{
    public Text firstPlayerName;
    public Text secondPlayerName;
    public Text firstPlayerScore;
    public Text secondPlayerScore;
    public Text winnerName;
    public string[] matchData = { " ", " ", " ", " ", " " };

    void Awake()
    {
        loadData();
        setValues();
    }

    void setValues() {
        firstPlayerName.text = matchData[0];
        secondPlayerName.text = matchData[1];
        firstPlayerScore.text = matchData[2];
        secondPlayerScore.text = matchData[3];
        winnerName.text = matchData[4];
    }

    void loadData() {
        string line;
        string directory = (Directory.GetParent(Environment.CurrentDirectory).ToString());
        StreamReader file = new StreamReader(Path.Combine(directory, @"GameData", "summary.txt"));

        for (int i = 0; (line = file.ReadLine()) != null; i++)
        {
            matchData[i] = line;
        }
        file.Close();
    }
}
