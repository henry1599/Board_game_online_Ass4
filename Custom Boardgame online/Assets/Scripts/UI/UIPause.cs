using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPause : MonoBehaviour
{
    public GameObject Canvas;
    void Start()
    {
        GameEvents.PAUSE += HandlePause;
    }
    void OnDestroy()
    {
        GameEvents.PAUSE -= HandlePause;
    }
    void HandlePause(bool isPause)
    {
        Collider[] cols = FindObjectsOfType<Collider>();
        foreach (Collider col in cols)
        {
            col.enabled = !isPause;
        }
        Canvas.SetActive(isPause);
    }
    [NaughtyAttributes.Button("Direct Pause")]
    public void Pause()
    {
        GameEvents.PAUSE?.Invoke(true);
    }
    [NaughtyAttributes.Button("Direct Unpause")]
    public void Unpause()
    {
        GameEvents.PAUSE?.Invoke(false);
    }
}
