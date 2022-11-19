using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInput : InputHandler
{
    protected Block targetBlock;

    public override void GetInput()
    {
        base.GetInput();
        targetBlock = null;
        StartCoroutine(MakeDecision());
    }

    protected virtual IEnumerator MakeDecision()
    {
        yield return null;
    }
}
