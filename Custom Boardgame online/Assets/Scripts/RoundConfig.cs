using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Round Config", fileName = "Round Config")]
public class RoundConfig : ScriptableObject
{
    public Color LogColor;
    public string Message;
    public int Round;
    [NaughtyAttributes.Button("Apply")]
    public void ApplyChanges()
    {
        GameManager.Round = Round;
        Debug.Log (string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>", (byte)(LogColor.r * 255f), (byte)(LogColor.g * 255f), (byte)(LogColor.b * 255f), Message));
    }
}
