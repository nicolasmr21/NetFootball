using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneAction : MonoBehaviour
{
	public InputField FieldNamePlayer;
	public InputField FieldIPServer;
	
	public void OnPlayOnline()
	{
		GameNetwork.ClientName = FieldNamePlayer.text;
        GameNetwork.SERVER_IP = FieldIPServer.text;
		SceneManager.LoadScene(1);
	}

    public void OnPlayLocal()
    {
        GameNetwork.ClientName = FieldNamePlayer.text;
        SceneManager.LoadScene(2);
    }
}
