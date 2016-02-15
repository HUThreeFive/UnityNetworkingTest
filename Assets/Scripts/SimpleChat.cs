/*
	All scripts are writen in really primitive way. As a networking beginner you should understand all
	things in this code and therefore you should not have any problems with converting/editing this code 
	for your own needs...

	OneManGames 2014 - Moopey
	email : omgdevelopersgroup@gmail.com
*/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;				// You need to import this to be able to use List<> in C#

[AddComponentMenu("Network/SimpleChat")]
[RequireComponent (typeof (NetworkView))]
public class SimpleChat : MonoBehaviour
{
	public Rect chatPosition;

	public string message;
	public List<string> messages;	//List of all messages
	
	SimpleNetwork sNetwork;			//NetworkScript
    public Move pMoveClient;
    public Move pMoveServer;
	
	void Start()
	{
		sNetwork = GetComponent<SimpleNetwork>();
        //pMove = GetComponent<Move>();
	}
	
	void Update()
	{
		if(messages.Count > (int)chatPosition.height / 25)		// Check how many messages you can fit in your chat window depending on chatPosition.height and divide by 25
		{
			messages.RemoveAt(0);										// This will remove the oldest message if new one is received and there is no more space in chat
		}
	}
	
	void OnGUI()
	{
		if(sNetwork.isConnected)											// Show chat if we are connected to the server
		{
			GUI.BeginGroup(chatPosition, "");
			GUI.Box(new Rect(0, 0, chatPosition.width, chatPosition.height), "Chat");
			
			for (var i = 0; i < messages.Count; i++)
				GUI.Label(new Rect(5, 30+(i*15), chatPosition.width - 10, 30), messages[i]);		// Display all messages in messages list (for every message in messages, display GUI.Label)
			
			message = GUI.TextField(new Rect(5, chatPosition.height - 30, chatPosition.width - 110, 25), message);
			if(GUI.Button(new Rect(chatPosition.width - 100, chatPosition.height - 30, 90, 25), "Send"))
			{
				if(message != "")
				{
					string tempMessage = sNetwork.playerName+"."+message;					// This will conver player name from SimpleNetwork script and current message into one string
					SendMessage(tempMessage);																// Player Name + message string converted into one will be sended to the SendMessage
				}
			}
			GUI.EndGroup();
		}
	}
	
	public void SendMessage(string msg)
	{
		GetComponent<NetworkView>().RPC ("ReciveMessage", RPCMode.All, msg);								// RPC Call to add message to the messages list



        message = "";                                                                                               // Detele message from TextField
    }
	
		
	[RPC]
	public void ReciveMessage(string msg)
	{
        string[] pieces = msg.Split('.');
        if (pieces[1] == "M")        //its a move
        {
            float x = Convert.ToSingle(pieces[2]);
            float z = Convert.ToSingle(pieces[3]);

            if (pieces[0] == sNetwork.playerName)
            {
                if (sNetwork.isServer)
                {
                    pMoveServer.MovePlayer(x, z);
                }
                else
                {
                    pMoveClient.MovePlayer(x, z);
                }
            }
            else
            {
                if (sNetwork.isServer)
                {
                    pMoveClient.MovePlayer(x, z);
                }
                else
                {
                    pMoveServer.MovePlayer(x, z);
                }
            }
        }
        else if(pieces[1] == "C")
        {
            string message = pieces[0] + " : " + pieces[2];
            messages.Add(message);                                                                                      // RPC call for message so everyone currently connected to the server can see it in chat
        }

	}
}