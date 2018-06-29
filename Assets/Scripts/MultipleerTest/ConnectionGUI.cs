using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class ConnectionGUI : MonoBehaviour {

	public string remoteIP = "127.0.0.1";
    public int remotePort = 25000;
    public int listenPort = 25000;
    public bool useNat = false;
    public string yourIP = "";
    public string yourPort = "";

    void OnGUI()
    {
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            if (GUI.Button(new Rect(10, 10, 100, 30), "Connect"))
            {
                Network.useNat = useNat;
                Network.Connect(remoteIP, remotePort);
            }

            if (GUI.Button(new Rect(10,50,100,30), "Start Server"))
            {
                Network.useNat = useNat;
                Network.InitializeServer(32, listenPort);
            }

            remoteIP = GUI.TextField(new Rect(120, 10, 100, 20), remoteIP);
            remotePort = Convert.ToInt32(GUI.TextField(new Rect(230, 10, 40, 20), remotePort.ToString()));

        } else
        {
            yourIP = Network.player.ipAddress;
            yourPort = Network.player.port.ToString();
            GUI.Label(new Rect(140, 20, 250, 40), "IP Adress: " + yourIP + ":" + yourPort);
            if (GUI.Button(new Rect(10, 10, 100, 50), "Disconnect"))
            {
                Network.Disconnect(200);
            }
        }
    }
}
