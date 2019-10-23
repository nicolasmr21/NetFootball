using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class ButtonHandler : MonoBehaviour
{
    public void OnClic(){
        Process proceso = new Process();
        proceso.StartInfo.FileName = @"C:\Users\CAMILO\Desktop\Claves.txt";
        proceso.Start();
   }
}
