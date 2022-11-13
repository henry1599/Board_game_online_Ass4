using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Renderer rend;
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
}
