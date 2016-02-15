/*
	All scripts are writen in really primitive way. As a networking beginner you should understand all
	things in this code and therefore you should not have any problems with converting/editing this code 
	for your own needs...

	OneManGames 2014 - Moopey
	email : omgdevelopersgroup@gmail.com
*/

using UnityEngine;
using System.Collections;

[AddComponentMenu("Network/SimpleNetwork")]
[RequireComponent (typeof (NetworkView))]
public class SimpleNetwork : MonoBehaviour
{
	public string connectionIP = "127.0.0.1";	//Direct connect IP
	public int connectionPort = 25001;				//Network Port
	
	public string playerName;
	[HideInInspector]
	public string tempName;							// Temp player name, prevent to access Host/Join menu before a player name will be typed 
	public string networkMessage;
	
	public bool isConnected;
    public bool isServer;
	
	SimpleChat sChat;										// Access to the chat script
	
	void Start()
	{
		DontDestroyOnLoad(this);
		sChat = GetComponent<SimpleChat>();
	}
	
	void Update()
	{
		NetworkMessageCheck();						// This will check if we are connected, disconnected, client or server and display a message depending on this
	}
	
	void OnGUI()
	{
		GUI.Box(new Rect(5, 5, 250, 25), "");
		GUI.Label(new Rect(10, 10, 250, 25), networkMessage);		// Used to display text message from NetworkMessageCheck
	
		if(playerName == "")				// If playerName is none then show "enter player name" gui
		{
			GUI.BeginGroup(new Rect((Screen.width / 2) - 100, (Screen.height / 2) - 25, 200, 50), "");
			tempName = GUI.TextField(new Rect(0, 0, 190, 25), tempName);
			if(GUI.Button(new Rect(75, 25, 50, 25), "Next"))
				playerName = tempName;
			GUI.EndGroup();
		}
		else
		{
			if(!isConnected)				// If we are not connect to the server, show 2 options to choose from (create server or direct connect to one)
			{
				GUI.BeginGroup(new Rect((Screen.width / 2) - 100, (Screen.height / 2) - 25, 200, 50), "");
				if(GUI.Button(new Rect(0, 0, 95, 25), "Host"))
					CreateServer();
				if(GUI.Button(new Rect(100, 0, 95, 25), "Join"))
					DirectConnect();
				GUI.EndGroup();
			}
			else
			{
				if(GUI.Button(new Rect(260, 5, 100, 25), "Disconnect"))
				{
					sChat.SendMessage(playerName+".C.disconnected!");
					Network.Disconnect();		// Disconnect from the network
				}
			}
		}
	}
	
	void NetworkMessageCheck()
	{
		if(Network.peerType == NetworkPeerType.Disconnected)
		{
			networkMessage = "Status: Disconnected from the server";
			isConnected = false;
		}
		else
		if(Network.peerType == NetworkPeerType.Connecting)
		{
			networkMessage = "Status: Connecting the server...";
		}
		else
		if(Network.peerType == NetworkPeerType.Client)
		{
			networkMessage = "Status: Connected as Client";
			isConnected = true;
		}
		else
		if(Network.peerType == NetworkPeerType.Server)
		{
			networkMessage = "Status: Connected as Server";
			isConnected = true;
		}
	}
	
	void OnConnectedToServer()
	{
		sChat.SendMessage(playerName+".C.joined!");		// Add message to the chat when player connect
	}
	
	void OnServerInitialized()
	{
		sChat.SendMessage(playerName+".C.joined!");		// Add message to the chat when player connect
	}
	
	void OnDisconnectedFromServer()
	{
		sChat.messages.Clear();												// Clear messages history
	}
	
	void CreateServer()																// Create simple local server
	{
		Network.InitializeServer(32, connectionPort, false);
        isServer = true;
	}
	
	void DirectConnect()															// Connect to the local server
	{
		Network.Connect(connectionIP, connectionPort);
        isServer = false;
	}
}