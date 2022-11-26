using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionUtils
{
    public static Dictionary<DataType, Func<Message, bool>> msgCallBack = new Dictionary<DataType, Func<Message, bool>>{};

    public static void SendMessage(Message msg) 
    {
        string data = BuildMessage(msg);
        if (data == "") return;
        Debug.Log(data);
        Connection.instance.SendMessageToServer(data);
    }

    public static void RegisterCallBack(DataType msgType, Func<Message, bool> msgFunc) 
    {
        msgCallBack[msgType] = msgFunc;
    } 

    public static void CallBack(Message msg) 
    {
        if (msg == null) return;
        if (msgCallBack.ContainsKey(msg.type)) 
        {
            msgCallBack[msg.type](msg);
        }
    }

    public static Message MessageHandler(byte[] rawData)
    {
        string text = System.Text.Encoding.UTF8.GetString(rawData);
        string[] data = text.Split('|');
        if (data.Length <= 1) {
            Debug.Log("Data is null");
            return null;
        }
        DataType dataType = (DataType)Int32.Parse(data[0]);
        Data dataBody;
        switch (dataType) 
        {
            case DataType.MOVERMENT:
                dataBody = PositionMessageHandler(data);
                break;
            case DataType.ACTIVE_STATUS:
                dataBody = ActiveMessageHandler(data);
                break;
            default:
                Debug.Log("Parse type error");
                return null;
        };
        Message msg = new Message();
        msg.type = dataType;
        msg.data = dataBody;
        return msg;
    }

    static PositionData PositionMessageHandler(string[] data)
    {
        PositionData psData = new PositionData();
        psData.id = data[1];
        psData.x = Int32.Parse(data[2]);
        psData.y = Int32.Parse(data[3]);
        return psData;
    }

    static ActiveData ActiveMessageHandler(string[] data)
    {
        ActiveData activeData = new ActiveData();
        activeData.id = data[1];
        activeData.active = bool.Parse(data[2]);
        return activeData;
    }

    static string BuildMessage(Message msg) 
    {
        var dataType = msg.type;
        switch (dataType)
        {
            case DataType.MOVERMENT:
                return BuildMovermentMessage(msg);
            case DataType.ACTIVE_STATUS:
                return BuildActiveMessage(msg);
            default:
                return "";
        }
    }

    static string BuildMovermentMessage(Message msg) 
    {
        PositionData data = msg.data as PositionData;
        var dataType = (int)msg.type;
        return dataType + "|" + data.id  + "|" + data.x  + "|" + data.y;
    }

    static string BuildActiveMessage(Message msg)
    {
        ActiveData data = msg.data as ActiveData;
        var dataType = (int)msg.type;
        return dataType + "|" + data.id + "|" + data.active;
    }
}
