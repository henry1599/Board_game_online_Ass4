using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RLAgent))]
public class RLInput : AIInput
{
    private RLAgent agent;

    protected override IEnumerator MakeDecision()
    {
        yield return new WaitUntil(() => GameManager.IsActive);
        if (agent == null)
        {
            agent = GetComponent<RLAgent>();
            agent.Init(character, OnReceiveAction);
        }
        agent.RequestDecision();
    }

    private void OnReceiveAction(Vector2Int target)
    {
        Block block = LevelManager.Instance.GetBlock(target);
        OnGetInput?.Invoke(character, block);
    }
}
