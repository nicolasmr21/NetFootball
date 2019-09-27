using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePhoton : MonoBehaviour
{
   
    public GameObject ball;
    public Boolean initial;
    public Text timeText;
    public float time = 60.0f;
    public int part;
    public int n;
    public Text waiting;



    // Start is called before the first frame update
    void Start()
    {
        part = 1;
        initial = false;
        Debug.Log("Nuevo");


        waiting.text = "";
    }

    private void Update()
    {
        time = time - 1 * Time.deltaTime;

        timeText.text = time.ToString("f0")+"";

        if (time < 0 && part == 1)
        {
            time = 60.0f;
            part++;


        }
        else if(time <0 && part==2) {
            
            SceneManager.LoadScene(3);
        }
    }


  
}
