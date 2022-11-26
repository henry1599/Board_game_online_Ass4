using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimaxInput : AIInput
{
    Dictionary<string, Block> CurrentCharacterBlock;

    protected override IEnumerator MakeDecision()
    {
        yield return new WaitUntil(() => GameManager.IsActive);
        CurrentCharacterBlock = new Dictionary<string, Block>();
        foreach (var kv in LevelManager.Instance.characters)
        {
            CurrentCharacterBlock.Add(kv.Key, kv.Value.CurrentBlock);
        }

        StartCoroutine(MinimaxRoot(GameManager.MinimaxCurrentMode == MinimaxMode.Hard ? 3 : 1));
    }

    private int MinimaxSearch(int depth, BlocksData data, string charId, int alpha, int beta, bool isMaximizeCharacter = false)
    {
        if (depth == 0)
        {
            return Utils.GetReward(data, character.Id, GameManager.MinimaxCurrentMode == MinimaxMode.Easy ? false : true);
        }
        int bestEvaluation;

        Block startBlock = CurrentCharacterBlock[charId];
        List<Vector2Int> moveableBlocks = Utils.GetMoveableBlocks(startBlock.data);

        string nextCharId = LevelManager.Instance.GetNextCharacter(charId).Id;
        if (isMaximizeCharacter)
        {
            bestEvaluation = -1000;
            foreach (Vector2Int moveableBlock in moveableBlocks)
            {
                // Forward
                Block targetBlock = LevelManager.Instance.GetBlock(moveableBlock);
                CurrentCharacterBlock[charId] = targetBlock;
                List<Vector2Int> movePath = Utils.GetMovePath(startBlock, targetBlock);

                // Evaluate
                var newBlocksData = Utils.GetNewBlocksData(data, charId, movePath);
                int evaluation = MinimaxSearch(depth - 1, newBlocksData, nextCharId, alpha, beta, !isMaximizeCharacter);
                bestEvaluation = Mathf.Max(bestEvaluation, evaluation);

                // Back
                CurrentCharacterBlock[charId] = startBlock;

                // Cut-off
                alpha = Mathf.Max(alpha, bestEvaluation);
                if (beta <= alpha)
                    return bestEvaluation;
            }
        }
        else
        {
            bestEvaluation = 1000;
            foreach (Vector2Int moveableBlock in moveableBlocks)
            {
                // Forward
                Block targetBlock = LevelManager.Instance.GetBlock(moveableBlock);
                CurrentCharacterBlock[charId] = targetBlock;
                List<Vector2Int> movePath = Utils.GetMovePath(startBlock, targetBlock);

                // Evaluate
                var newBlocksData = Utils.GetNewBlocksData(data, charId, movePath);
                int evaluation = MinimaxSearch(depth - 1, newBlocksData, nextCharId, alpha, beta, !isMaximizeCharacter);
                bestEvaluation = Mathf.Min(bestEvaluation, evaluation);

                // Back
                CurrentCharacterBlock[charId] = startBlock;

                // Cut-off
                beta = Mathf.Min(beta, bestEvaluation);
                if (beta <= alpha)
                    return bestEvaluation;
            }
        }

        return bestEvaluation;
    }
    private IEnumerator MinimaxRoot(int depth)
    {
        if (depth < 1)
            depth = 1;

        Block bestTargetBlock = null;
        int bestEvaluation = -1000;
        var currentblocksData = new BlocksData(LevelManager.Instance.blocksData);
        var startBlock = CurrentCharacterBlock[character.Id];

        foreach (Vector2Int moveableBlock in character.MoveableBlocks)
        {
            yield return null;
            // forward 
            Block targetBlock = LevelManager.Instance.GetBlock(moveableBlock);
            List<Vector2Int> movePath = Utils.GetMovePath(startBlock, targetBlock);
            var newBlocksData = Utils.GetNewBlocksData(currentblocksData, character.Id, movePath);

            // compare
            int evaluation = MinimaxSearch(depth - 1, newBlocksData, character.Id, -1000, 1000);
            if (evaluation >= bestEvaluation)
            {
                bestTargetBlock = targetBlock;
                bestEvaluation = evaluation;
            }

            // backward
        }

        OnGetInput?.Invoke(character, bestTargetBlock);
    }
}
