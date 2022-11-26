using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class UIRound : MonoBehaviour
{
    public TMP_Text text;
    StringBuilder stringBuilder = new StringBuilder();
    void Awake()
    {
        stringBuilder.Clear();
        stringBuilder.AppendFormat("ROUND : {0}", GameManager.Round);
        text.text = stringBuilder.ToString();
        GameManager.ON_ROUND_CHANGED += HandleRoundChanged;
    }
    void OnDestroy()
    {
        GameManager.ON_ROUND_CHANGED -= HandleRoundChanged;
    }
    void HandleRoundChanged(int round)
    {
        stringBuilder.Clear();
        stringBuilder.AppendFormat("ROUND : {0}", round);
        text.text = stringBuilder.ToString();
    }
    public void OnBackButtonClick()
    {
        GameEvents.LOAD_SCENE?.Invoke("Main");
    }
}
