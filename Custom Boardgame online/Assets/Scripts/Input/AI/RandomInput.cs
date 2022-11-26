using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInput : AIInput
{
    protected override IEnumerator MakeDecision()
    {
        yield return new WaitUntil(() => GameManager.IsActive);
        yield return new WaitForSeconds(0.25f);
        List<Vector2Int> moveableBlocks = character.MoveableBlocks;
        int targetIndex = Random.Range(0, moveableBlocks.Count);
        Block targetBlock = LevelManager.Instance.GetBlock(moveableBlocks[targetIndex]);
        OnGetInput?.Invoke(character, targetBlock);
    }
}
