using UnityEngine;
using System.Collections;

 

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

 

public class UDPSend : MonoBehaviour
{
    private static int localPort;

    // prefs
    private string IP = "192.168.1.6";
    public int port = 5566;
    // define in init
    // define in init
    // "connection" things
    IPEndPoint remoteEndPoint;
    UdpClient client;

    // gui
    public string strMessage = "";


    // call it from shell (as program)
    //private static void Main()
    // {
    //UDPSend sendObj = new UDPSend();
    // sendObj.init();



    // testing via console
    // sendObj.inputFromConsole();



    // as server sending endless
    //sendObj.sendEndless(" endless infos \n");



    //}
    // start from unity3d
    public void Start()
    {
        init();
    }
    

 
void Update()
  {
    if (!string.IsNullOrEmpty(strMessage) && client != null)
        {
            sendString(strMessage); // Send the current message
            strMessage = "";        // IMPORTANT: Clear the message after sending
                                    // This stops it from being sent repeatedly every frame
        }
  }


    // OnGUI
    void OnGUI()
    {
        Rect rectObj = new Rect(40, 380, 200, 400);
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        GUI.Box(rectObj, "# UDPSend-Data\n" + IP + ":" + port + " #\n"
                        + "shell> nc -lu " + IP + " " + port + " \n"
                        , style);
                        
        // GUI for manually setting and sending message
        // This GUI element will overwrite strMessage, so be careful if Player.cs is also writing to it.
        strMessage = GUI.TextField(new Rect(160, 360, 140, 20), strMessage);
        if (GUI.Button(new Rect(310, 360, 40, 20), "send"))
        {
            sendString(strMessage); // Send the message from the GUI
            // strMessage = ""; // You might want to clear it here too if it's a one-time send from GUI
        }
    }

    // init
    public void init()
    {
        // Endpunkt definieren, von dem die Nachrichten gesendet werden.
        print("UDPSend.init()");

        // define
        // IP = "192.168.1.6";
        // port = 5566;

        // ----------------------------
        // Senden
        // ----------------------------
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        client = new UdpClient();

        // status
        try
        {
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
            client = new UdpClient();
            print("Sending to " + IP + " : " + port);
            print("Testing: nc -lu " + IP + " : " + port);
        }
        catch (Exception err)
        {
            Debug.LogError($"UDP Sender Initialization Error: {err.Message}");
            enabled = false; // Disable the script if initialization fails
        }

    }

    // sendData
    public void sendString(string message)
    {
        try
        {
            // string[] strMessage = new string[2];
            // strMessage = message.Split(",");
            // float mess1 = float.Parse(strMessage[0]);
            // float mess2 = float.Parse(strMessage[1]);

            // Daten mit der UTF8-Kodierung in das Binärformat kodieren.
            // byte[] data = BitConverter.GetBytes(mess1);
            byte[] data = Encoding.UTF8.GetBytes(message);


            // Den message zum Remote-Client senden.
            client.Send(data, data.Length, remoteEndPoint);
            //Debug.Log("Sending length: " + data.Length);
            //            Debug.Log("Tx : " + message);



        }
        catch (Exception err)
        {
            print(err.ToString());
            Debug.LogError($"UDP Send Error: {err.Message}");
        }
    }
void OnApplicationQuit()
    {
        if (client != null)
        {
            client.Close();
            Debug.Log("UDP Sender closed.");
        }
    }
}