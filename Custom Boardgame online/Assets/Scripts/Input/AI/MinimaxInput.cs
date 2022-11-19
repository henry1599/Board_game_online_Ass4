using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimaxInput : AIInput
{
    Dictionary<string, Block> CurrentCharacterBlock;

    protected override IEnumerator MakeDecision()
    {
        yield return null;
        CurrentCharacterBlock = new Dictionary<string, Block>();
        foreach (var kv in LevelManager.Instance.characters)
        {
            CurrentCharacterBlock.Add(kv.Key, kv.Value.CurrentBlock);
        }

        StartCoroutine(FindNextMove());
    }

    private IEnumerator FindNextMove()
    {
        var currentblocksData = new BlocksData(LevelManager.Instance.blocksData);
        var startBlock = CurrentCharacterBlock[character.Id];
        Block bestTargetBlock = null;
        int bestEvaluation = -100;
        List<int> evaluationList = new List<int>();
        foreach (Vector2Int moveableBlock in character.MoveableBlocks)
        {
            yield return null;
            Block targetBlock = LevelManager.Instance.GetBlock(moveableBlock);
            List<Vector2Int> movePath = Utils.GetMovePath(startBlock, targetBlock);
            var newBlocksData = Utils.GetNewBlocksData(currentblocksData, character.Id, movePath);
            int evaluation = -Search(newBlocksData, character.Id, 2);
            // int evaluation = Utils.GetReward(newBlocksData, character.Id);
            if (evaluation >= bestEvaluation)
            {
                bestTargetBlock = targetBlock;
                bestEvaluation = evaluation;
            }
            evaluationList.Add(evaluation);
        }
        Debug.Log($"x: {bestTargetBlock.data.Idx.x}, y: {bestTargetBlock.data.Idx.y} -> evaluation: {bestEvaluation}");
        // string debug = "";
        // foreach (int eval in evaluationList)
        // {
        //     debug += eval + " ";
        // }
        // Debug.Log($"Evaluations: {debug}");
        OnGetInput?.Invoke(character, bestTargetBlock);
    }

    private int Search(BlocksData currentblocksData, string charId, int depth = 0, bool isMaximizeCharacter = false)
    {
        if (depth == 0)
        {
            int flag = isMaximizeCharacter ? 1 : -1;
            return Utils.GetReward(currentblocksData, charId) * flag;
        }

        int bestEvaluation;

        string nextCharId = LevelManager.Instance.GetNextCharacter(charId).Id;
        Block startBlock = CurrentCharacterBlock[nextCharId];
        List<Vector2Int> moveableBlocks = Utils.GetMoveableBlocks(startBlock.data);
        if (isMaximizeCharacter)
        {
            bestEvaluation = -1000;
            foreach (Vector2Int moveableBlock in character.MoveableBlocks)
            {
                // Forward
                Block targetBlock = LevelManager.Instance.GetBlock(moveableBlock);
                CurrentCharacterBlock[nextCharId] = targetBlock;
                List<Vector2Int> movePath = Utils.GetMovePath(startBlock, targetBlock);

                // Evaluate
                var newBlocksData = Utils.GetNewBlocksData(currentblocksData, nextCharId, movePath);
                int evaluation = Search(newBlocksData, nextCharId, depth - 1);
                bestEvaluation = Mathf.Max(bestEvaluation, evaluation);

                // Back
                CurrentCharacterBlock[nextCharId] = startBlock;
            }
        }
        else
        {
            bestEvaluation = 1000;
            foreach (Vector2Int moveableBlock in character.MoveableBlocks)
            {
                // Forward
                Block targetBlock = LevelManager.Instance.GetBlock(moveableBlock);
                CurrentCharacterBlock[nextCharId] = targetBlock;
                List<Vector2Int> movePath = Utils.GetMovePath(startBlock, targetBlock);

                // Evaluate
                var newBlocksData = Utils.GetNewBlocksData(currentblocksData, nextCharId, movePath);
                int evaluation = Search(newBlocksData, nextCharId, depth - 1);
                bestEvaluation = Mathf.Min(bestEvaluation, evaluation);

                // Back
                CurrentCharacterBlock[nextCharId] = startBlock;
            }
        }

        return bestEvaluation;
    }
}
