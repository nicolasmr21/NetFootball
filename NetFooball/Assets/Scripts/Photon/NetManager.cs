using Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetManager : PunBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("v1.0");
    }

    public override void OnConnectedToPhoton()
    {
        print("Conectado a photon");
    }


    public override void OnConnectedToMaster()
    {
        print("Conectado al master");

        var options = new RoomOptions();
        options.maxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("Net", options, TypedLobby.Default);
    }

    public override void OnCreatedRoom() {
        
        print("Se crea la room");

    }

    public override void OnJoinedRoom()
    {
        print("join room");

        PhotonNetwork.Instantiate("bot", new Vector3(0, 2, -12), Quaternion.identity, 0);

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
