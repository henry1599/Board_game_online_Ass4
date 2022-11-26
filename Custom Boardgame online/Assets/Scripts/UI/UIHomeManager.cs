using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHomeManager : MonoBehaviour
{
    public void OnStartButtonClick()
    {
        GameEvents.LOAD_SCENE?.Invoke("Gameplay");
        GameManager.StartGame(GameMode.Random);
    }

    public void OnRandomButtonClick()
    {
        GameEvents.LOAD_SCENE?.Invoke("Gameplay");
        GameManager.StartGame(GameMode.Random);
    }

    public void OnMinimaxEasyButtonClick()
    {
        GameEvents.LOAD_SCENE?.Invoke("Gameplay");
        GameManager.MinimaxCurrentMode = MinimaxMode.Easy;
        GameManager.StartGame(GameMode.Minimax);
    }

    public void OnMinimaxNormalButtonClick()
    {
        GameEvents.LOAD_SCENE?.Invoke("Gameplay");
        GameManager.MinimaxCurrentMode = MinimaxMode.Normal;
        GameManager.StartGame(GameMode.Minimax);
    }

    public void OnMinimaxHardButtonClick()
    {
        GameEvents.LOAD_SCENE?.Invoke("Gameplay");
        GameManager.MinimaxCurrentMode = MinimaxMode.Hard;
        GameManager.StartGame(GameMode.Minimax);
    }

    public void OnMachineLearningButtonClick()
    {
        GameEvents.LOAD_SCENE?.Invoke("Gameplay");
        GameManager.StartGame(GameMode.MachineLearning);
    }

    public void OnCompareButtonClick()
    {
        GameEvents.LOAD_SCENE?.Invoke("Gameplay");
        GameManager.StartGame(GameMode.Compare);
    }

    public void OnOnlineButtonClick()
    {
        GameEvents.LOAD_SCENE?.Invoke("Gameplay");
        GameManager.StartGame(GameMode.Online);
    }
}
