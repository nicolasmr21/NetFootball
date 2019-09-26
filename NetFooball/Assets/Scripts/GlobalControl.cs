using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;
    public GameObject player1;
    public GameObject player2;
    public GameObject ball;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        player1 = GlobalControl.Instance.player1;
        player2 = GlobalControl.Instance.player2;
        ball = GlobalControl.Instance.ball;
    }

    public void saveData()
    {
        GlobalControl.Instance.player1 = player1;
        GlobalControl.Instance.player2 = player2;
        GlobalControl.Instance.ball = ball;
    }
}
