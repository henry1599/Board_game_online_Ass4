using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    Random,
    Minimax,
    MachineLearning,
    Online
}

public class GameManager : MonoBehaviour
{
    public static GameMode CurrentMode = GameMode.Minimax;
    public static string PlayerId = "0";
}
