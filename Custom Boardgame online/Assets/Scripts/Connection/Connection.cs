using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NativeWebSocket;

public class Connection : MonoBehaviour
{
    public static Connection instance;
    WebSocket websocket;
    // Start is called before the first frame update
    async void Awake()
    {
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            websocket = new WebSocket("ws://localhost:8080");

            websocket.OnOpen += () =>
            {
                Debug.Log("Connection open!");
            };

            websocket.OnError += (e) =>
            {
                Debug.Log("Error! " + e);
            };

            websocket.OnClose += (e) =>
            {
                Debug.Log("Connection closed!");
            };

            websocket.OnMessage += (data) =>
            {
                var msg = ConnectionUtils.MessageHandler(data);
                ConnectionUtils.CallBack(msg);
            };
            await websocket.Connect();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        websocket.DispatchMessageQueue();
    }

    public void SendMessageToServer(string data) 
    {
        websocket.SendText(data);
    }
}
