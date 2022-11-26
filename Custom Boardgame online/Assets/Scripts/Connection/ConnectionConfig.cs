using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DataType
{
    MOVERMENT,
    ACTIVE_STATUS
}

public abstract class Data{}

public class Message {
    public DataType type;
    public Data data;
}

public class PositionData : Data
{
    public string id;
    public int x;
    public int y;
}

public class ActiveData : Data
{
    public string id;
    public bool active;
}