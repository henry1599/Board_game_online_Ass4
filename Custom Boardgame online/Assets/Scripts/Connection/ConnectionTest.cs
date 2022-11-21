using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ConnectionUtils.RegisterCallBack(DataType.MOVERMENT, CallBack);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            var pos = new PositionData();
            pos.id = "1234456";
            pos.x = Random.Range(0, 10);
            pos.y = Random.Range(0, 10);
            var msg = new Message();
            msg.type = DataType.MOVERMENT;
            msg.data = pos;
            ConnectionUtils.SendMessage(msg);
        }
    }

    bool CallBack(Message msg) 
    {
        var data = msg.data as PositionData;
        Debug.Log("Callback" + msg.type + data.x + data.y);
        return true;
    }
}
