using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHomeManager : MonoBehaviour
{
    public void OnStartButtonClick()
    {
        GameEvents.LOAD_SCENE?.Invoke("Gameplay");
    }
}
