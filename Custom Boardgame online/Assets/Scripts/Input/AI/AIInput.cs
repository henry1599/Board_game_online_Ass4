using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInput : InputHandler
{

    public override void GetInput()
    {
        base.GetInput();
        StartCoroutine(MakeDecision());
    }

    protected virtual IEnumerator MakeDecision()
    {
        yield return null;
    }
}
