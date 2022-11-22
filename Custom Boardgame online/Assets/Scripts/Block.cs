using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
public class Block : MonoBehaviour
{
    public Renderer rend;
    public BlockData data;
    public static Action<Block> OnClick;
    public void SetColor(float[] color)
    {
        this.rend.material.SetColor("_BaseColor", new Color(color[0], color[1], color[2], color[3]));
        this.rend.material.SetColor("_EmissionColor", new Color(color[0], color[1], color[2], color[3]));        
    }
    public void SetColor(Color color)
    {
        this.rend.material.SetColor("_BaseColor", new Color(color.r, color.g, color.b, color.a));
        this.rend.material.SetColor("_EmissionColor", new Color(color.r, color.g, color.b, color.a));
    }
    private void OnMouseDown()
    {
        OnClick?.Invoke(this);
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (data != null)
        {
            Handles.Label(transform.position + Vector3.up * 0.5f, $"x:{data.Idx.x},y:{data.Idx.y}");
        }
    }
#endif
}
