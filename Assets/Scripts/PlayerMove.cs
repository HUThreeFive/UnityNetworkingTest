﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerMove : MonoBehaviour
{
	void Update()
	{
//		if (!isLocalPlayer)
//			return;
        Move();
//		var x = Input.GetAxis("Horizontal")*0.1f;
//		var z = Input.GetAxis("Vertical")*0.1f;
//
//		transform.Translate(x, 0, z);
	}

    public void Move()
    {
        var x = Input.GetAxis("Horizontal")*0.1f;
        var z = Input.GetAxis("Vertical")*0.1f;

        transform.Translate(x, 0, z);
    }
//	public override void OnStartLocalPlayer()
//	{
//		GetComponent<MeshRenderer>().material.color = Color.green;
//	}
}