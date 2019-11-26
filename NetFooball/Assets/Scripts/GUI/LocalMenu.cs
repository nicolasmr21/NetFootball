using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LocalMenu : MonoBehaviour
{
    public void onPlayAgainstCOM(){
        SceneManager.LoadScene(1);
    }

    public void onCOMVSCOM(){
        SceneManager.LoadScene(6);
    }
}
