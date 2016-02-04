using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Chat : NetworkBehaviour {

	public InputField chatInput;
	public Text chatView;

	private List<string> chatHistory = new List<string>();

	public void SendChat()
	{
		string currentMessage = chatInput.text;
		chatHistory.Add (currentMessage);

		string history = string.Empty;

		foreach (string c in chatHistory) {
			history = history + c + System.Environment.NewLine;
		}
		chatView.text = history;
		Debug.Log (currentMessage);

		CmdChatMessage(currentMessage);
	}

	//client to server -- called on server
	[Command]
	private void CmdChatMessage(string message)
	{
		RpcReceiveChatMessage(message);
		chatHistory.Add(message);
	}

	//server to client -- called on client
	[ClientRpc]
	void RpcReceiveChatMessage(string message)
	{
		chatHistory.Add(message);
	}
}
