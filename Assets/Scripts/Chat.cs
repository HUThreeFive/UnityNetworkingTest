using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Chat : NetworkManager {

	public InputField chatInput;
	public Text chatView;
	//private bool isAClient;
    private List<string> history = new List<string>();
	//private SyncListString chatHistory = new SyncListString();

	public override void OnStartClient(NetworkClient mClient)
	{
		//chatHistory.Callback = OnStringChanged;
        base.OnStartClient(mClient);

        mClient.RegisterHandler((short)MyMessages.MyMessageTypes.CHAT_MESSAGE, OnClientChatMessage);
		//isAClient = true;
	}

	public override void OnStartServer()
	{
        base.OnStartServer();
        NetworkServer.RegisterHandler((short)MyMessages.MyMessageTypes.CHAT_MESSAGE, OnServerChatMessage);
		//NetworkServer.Listen (7777);
		//NetworkServer.RegisterHandler (MsgType.Command, MsgRcvd);
		//isAClient = false;
	}

	public void SendChat()
	{
		string currentMessage = "hey";

		//if (isAClient) 

            MyMessages.ChatMessage msg = new MyMessages.ChatMessage();
            msg.message = currentMessage;
            NetworkManager.singleton.client.Send((short)MyMessages.MyMessageTypes.CHAT_MESSAGE, msg);
            //RegisterHost(currentMessage);
		 


		//CmdChatMessage(currentMessage);
	}

    private void OnServerChatMessage(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<MyMessages.ChatMessage>();

        MyMessages.ChatMessage chat = new MyMessages.ChatMessage();
        chat.message = msg.message;

        NetworkServer.SendToAll((short)MyMessages.MyMessageTypes.CHAT_MESSAGE, chat);
    }

    private void OnClientChatMessage(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<MyMessages.ChatMessage>();

        history.Add(msg.message);
        string h = string.Empty;
        foreach (string c in history)
        {
            h = h + c + System.Environment.NewLine;
        }

        chatView.text = h;
    }


//	private void MsgRcvd(NetworkMessage netMsg)
//	{
//		var msg = netMsg.ReadMessage<RegisterHostMessage>();
//        Debug.Log(msg);
//	}
//
//	private void OnStringChanged(SyncListString.Operation op, int index)
//	{
//		string history = string.Empty;
//		foreach (string c in chatHistory) {
//			history = history + c + System.Environment.NewLine;
//		}
//		chatView.text = history;
//	}
//
//	public NetworkClient client;
//
//	public const short registerHostMsgId = 9;
//
//	public void RegisterHost(string name)
//	{
//		var msg = new RegisterHostMessage ();
//		msg.gameName = name;
//		msg.comment = "test";
//		msg.passwordProtected = false;
//
//		client.Send (registerHostMsgId, msg);
//	}
//
	//client to server -- called on server
	/*[Command]
	private void CmdChatMessage(string message)
	{
		chatHistory.Add (message);
	}

	//server to client -- called on client
	[ClientRpc]
	void RpcReceiveChatMessage(string message)
	{
		chatHistory.Add(message);
		UpdateChatHistory (message);
	}*/
}

//class RegisterHostMessage : MessageBase
//{
//	public string gameName;
//	public string comment;
//	public bool passwordProtected;
//
//}     

public class MyMessages
{

    public enum MyMessageTypes
    {
        CHAT_MESSAGE = 1000
    }

    public class ChatMessage : MessageBase
    {
        public string message;
    }

}
