using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameNetwork : MonoBehaviour
{

	// Cliente
	public static string ClientName = "Client";
	// ip del servidor
	public static string SERVER_IP = "localhost";
    // Puerto del servidor
    private readonly int SERVER_PORT = 13000;
	// Data del mensage
	private byte[] bufferDataReceive = new byte[1024];
	// Socket del cliente
	private Socket clientSocket = new Socket(
		AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp
	);
    // Update se llama una vez por frame
    void Update()
    {
		LoopConnection();

		if ( clientSocket.Connected)
			LoopSend();

	}
	// Loop para conectar con el servidor
	private void LoopConnection()
	{
		// Si aún no está conectado
		if (!clientSocket.Connected)
		{
			try
			{
				clientSocket.Connect(SERVER_IP, SERVER_PORT);
			}
			catch (SocketException)
			{
				SceneManager.LoadScene(0);
			}
		}
	}
	// Cuando ya está conectado, metodo para enviar datos
	private void LoopSend()
	{

        GameObject p = gameObject.GetComponent<Game>().player1;
        GameObject b = gameObject.GetComponent<Game>().ball;

        // Recibir y enviar mensaje
        try
        {
         

            // Limpiar los datos
            Array.Clear(bufferDataReceive, 0, bufferDataReceive.Length);

            // Tamaño del mensaje recibido
            int receiveSize = clientSocket.Receive(bufferDataReceive);
            //Mensaje recibido 
            string dataReceive = Encoding.ASCII.GetString(bufferDataReceive);
            //
            if(dataReceive.Contains("NoPlayer2")) {
                SceneManager.LoadScene(1);
                clientSocket.Close();
                LoopConnection();
            }

            if (dataReceive.Contains("Wait"))
            {
                b.GetComponent<Ball>().score1 = 0;
                b.GetComponent<Ball>().score2 = 0;
                gameObject.GetComponent<Game>().time = 60.0f;
                gameObject.GetComponent<Game>().waiting.text = "Esperando jugadores...";

            }
            else {
                gameObject.GetComponent<Game>().waiting.text = "";

            }

            gameObject.GetComponent<Game>().UpdatePlay(dataReceive);

            // Data a enviar
            string dataSend = ClientName + "|" + p.transform.position.x + "|" + p.transform.position.y + "|" + p.transform.position.z
                + "|" + p.transform.rotation.x + "|" + p.transform.rotation.y + "|" + p.transform.rotation.z + "|" + p.transform.rotation.w
                + "|" + b.transform.position.x + "|" + b.transform.position.y + "|" + b.transform.position.z + "|" +p.GetComponent<Player>().score;
           ;
        
			//
			//Debug.Log("Mensage recibido: " + dataReceive);
			// Mensage para enviar
			byte[] bufferDataSend = Encoding.ASCII.GetBytes(dataSend);
			// Enviar mensaje al servidor
			clientSocket.Send(bufferDataSend);
			//
		}
		catch (SocketException)
		{
			clientSocket.Shutdown(SocketShutdown.Both);
			clientSocket.Close();
			SceneManager.LoadScene(0);

		}
	}
}
