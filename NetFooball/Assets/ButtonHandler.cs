using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class ButtonHandler : MonoBehaviour
{
    public void OnClic(){
        Process proceso = new Process();
        proceso.StartInfo.FileName = @"C:\Users\estep\OneDrive\Documentos\GitHub\NetworksProject\NetFotball\AppBuild\Udp_Client\dist\udp_client.exe";
        proceso.Start();
   }
}
