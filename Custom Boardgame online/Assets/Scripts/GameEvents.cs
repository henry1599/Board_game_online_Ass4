using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents
{
    public static System.Action<string> LOAD_SCENE;
    public static System.Action<bool> PAUSE;
}
public class GameConstants
{
    public static readonly float TRANSITION_IN_DURATION = 1.5f;
    public static readonly float TRANSITION_OUT_DURATION = 1;
}
