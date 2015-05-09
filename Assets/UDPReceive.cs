/*
    Used the code from http://forum.unity3d.com/threads/simple-udp-implementation-send-read-via-mono-c.15900/
    -----------------------
    UDP-Receive (send to)
    -----------------------
    // [url]http://msdn.microsoft.com/de-de/library/bb979228.aspx#ID0E3BAC[/url]
   
   
    // > receive
    // 127.0.0.1 : 8051
   
    // send
    // nc -u 127.0.0.1 8051
 
*/
using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour
{
	
	// receiving Thread
	Thread receiveThread;
	
	// udpclient object
	UdpClient client;

	public bool Activated { get; set; }

	// public
	// public string IP = "127.0.0.1"; default local
	public int port; // define > init
	
	// infos
	public string lastReceivedUDPPacket = "";
	public string allReceivedUDPPackets = ""; // clean up this from time to time!

	// start from shell
	private static void Main ()
	{
		UDPReceive receiveObj = new UDPReceive ();
		receiveObj.init ();
		
		string text = "";
		do {
			text = Console.ReadLine ();
		} while(!text.Equals("exit"));
	}
	// start from unity3d
	public void Start ()
	{
		init ();
	}

	// OnGUI (only use for debugging, too expensive)
	void OnGUI ()
	{
//		Rect rectObj=new Rect(10,500,200,50);
//		GUIStyle style = new GUIStyle();
//		style.alignment = TextAnchor.UpperLeft;
//		GUI.Box(rectObj,"\nLast Packet: "+ lastReceivedUDPPacket
//		  		        ,style);
	}
	
	// init
	private void init ()
	{
		// Register receiver
		GameSystem_8Ball.RegisterReceiver (this);

		// Endpunkt definieren, von dem die Nachrichten gesendet werden.
		print ("UDPSend.init()");
		
		// define port
		port = 29129;
		Activated = false;

		// status
		print ("Sending to 127.0.0.1 : " + port);
		print ("Test-Sending to this Port: nc -u 127.0.0.1  " + port + "");
		
		
		client = new UdpClient (port);

		// ----------------------------
		// Abhören
		// ----------------------------
		// Lokalen Endpunkt definieren (wo Nachrichten empfangen werden).
		// Einen neuen Thread für den Empfang eingehender Nachrichten erstellen.
		receiveThread = new Thread (
			new ThreadStart (ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start ();
		
	}

	private void ReceiveData ()
	{
		while (true)
		while (Activated && client.Available > 0) {
				try {
					// Bytes empfangen.
					IPEndPoint anyIP = new IPEndPoint (IPAddress.Any, 0);
					byte[] data = client.Receive (ref anyIP);
					
					// Bytes mit der UTF8-Kodierung in das Textformat kodieren.
					string text = Encoding.UTF8.GetString (data);
					
					// Den abgerufenen Text anzeigen.
					//print (">> " + text);
					
					// latest UDPpacket
					lastReceivedUDPPacket = text;
					
					// ....
					// allReceivedUDPPackets = allReceivedUDPPackets + text;
					
				} catch (Exception err) {
					print (err.ToString ());
				}
			}
	}
	
	// getLatestUDPPacket
	// cleans up the rest
	public string getLatestUDPPacket ()
	{
		allReceivedUDPPackets = "";
		return lastReceivedUDPPacket;
	}

//	void OnDisable ()
//	{ 
//		if (receiveThread != null) 
//			receiveThread.Abort (); 
//		
//		client.Close (); 
//	} 
}
