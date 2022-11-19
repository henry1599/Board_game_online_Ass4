using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static List<Vector2Int> GetMoveableBlocks(BlockData data)
    {
        int x = data.Idx.x;
        int y = data.Idx.y;
        List<Vector2Int> moveableBlocks = new List<Vector2Int>();
        for (int i = 0; i < 10; i++)
        {
            if (i == x) continue;

            moveableBlocks.Add(new Vector2Int(i, y));
        }

        for (int i = 0; i < 10; i++)
        {
            if (i == y) continue;

            moveableBlocks.Add(new Vector2Int(x, i));
        }
        int temp_x;
        int temp_y;

        temp_x = x;
        temp_y = y;
        while (temp_x > 0 && temp_y > 0)
        {
            temp_x--;
            temp_y--;
            moveableBlocks.Add(new Vector2Int(temp_x, temp_y));
        }
        temp_x = x;
        temp_y = y;
        while (temp_x > 0 && temp_y < 10 - 1)
        {
            temp_x--;
            temp_y++;
            moveableBlocks.Add(new Vector2Int(temp_x, temp_y));
        }
        temp_x = x;
        temp_y = y;
        while (temp_x < 10 - 1 && temp_y > 0)
        {
            temp_x++;
            temp_y--;
            moveableBlocks.Add(new Vector2Int(temp_x, temp_y));
        }
        temp_x = x;
        temp_y = y;
        while (temp_x < 10 - 1 && temp_y > 0)
        {
            temp_x++;
            temp_y--;
            moveableBlocks.Add(new Vector2Int(temp_x, temp_y));
        }
        temp_x = x;
        temp_y = y;
        while (temp_x < 10 - 1 && temp_y < 10 - 1)
        {
            temp_x++;
            temp_y++;
            moveableBlocks.Add(new Vector2Int(temp_x, temp_y));
        }
        return moveableBlocks;
    }

    public static List<Vector2Int> GetMovePath(Block currentBlock, Block moveToBlock)
    {
        List<Vector2Int> movePath = new List<Vector2Int>();

        if (currentBlock.data.Idx.x == moveToBlock.data.Idx.x && currentBlock.data.Idx.y == moveToBlock.data.Idx.y)
        {
            Debug.Log("Player dont move ???");
        }
        else if (currentBlock.data.Idx.x < moveToBlock.data.Idx.x && currentBlock.data.Idx.y == moveToBlock.data.Idx.y)
        {
            for (int i = currentBlock.data.Idx.x + 1; i <= moveToBlock.data.Idx.x; i++)
                movePath.Add(new Vector2Int(i, moveToBlock.data.Idx.y));
        }
        else if (currentBlock.data.Idx.x > moveToBlock.data.Idx.x && currentBlock.data.Idx.y == moveToBlock.data.Idx.y)
        {
            for (int i = currentBlock.data.Idx.x - 1; i >= moveToBlock.data.Idx.x; i--)
                movePath.Add(new Vector2Int(i, moveToBlock.data.Idx.y));
        }
        else if (currentBlock.data.Idx.x == moveToBlock.data.Idx.x && currentBlock.data.Idx.y < moveToBlock.data.Idx.y)
        {
            for (int i = currentBlock.data.Idx.y + 1; i <= moveToBlock.data.Idx.y; i++)
                movePath.Add(new Vector2Int(moveToBlock.data.Idx.x, i));
        }
        else if (currentBlock.data.Idx.x == moveToBlock.data.Idx.x && currentBlock.data.Idx.y > moveToBlock.data.Idx.y)
        {
            for (int i = currentBlock.data.Idx.y - 1; i >= moveToBlock.data.Idx.y; i--)
                movePath.Add(new Vector2Int(moveToBlock.data.Idx.x, i));
        }
        else if (currentBlock.data.Idx.x < moveToBlock.data.Idx.x && currentBlock.data.Idx.y < moveToBlock.data.Idx.y)
        {
            int step = Mathf.Abs(currentBlock.data.Idx.x - moveToBlock.data.Idx.x);
            for (int i = 1; i <= step; i++)
                movePath.Add(new Vector2Int(currentBlock.data.Idx.x + i, currentBlock.data.Idx.y + i));
        }
        else if (currentBlock.data.Idx.x > moveToBlock.data.Idx.x && currentBlock.data.Idx.y > moveToBlock.data.Idx.y)
        {
            int step = Mathf.Abs(currentBlock.data.Idx.x - moveToBlock.data.Idx.x);
            for (int i = 1; i <= step; i++)
                movePath.Add(new Vector2Int(currentBlock.data.Idx.x - i, currentBlock.data.Idx.y - i));
        }
        else if (currentBlock.data.Idx.x > moveToBlock.data.Idx.x && currentBlock.data.Idx.y < moveToBlock.data.Idx.y)
        {
            int step = Mathf.Abs(currentBlock.data.Idx.x - moveToBlock.data.Idx.x);
            for (int i = 1; i <= step; i++)
                movePath.Add(new Vector2Int(currentBlock.data.Idx.x - i, currentBlock.data.Idx.y + i));
        }
        else if (currentBlock.data.Idx.x < moveToBlock.data.Idx.x && currentBlock.data.Idx.y > moveToBlock.data.Idx.y)
        {
            int step = Mathf.Abs(currentBlock.data.Idx.x - moveToBlock.data.Idx.x);
            for (int i = 1; i <= step; i++)
                movePath.Add(new Vector2Int(currentBlock.data.Idx.x + i, currentBlock.data.Idx.y - i));
        }

        return movePath;
    }

    public static int GetReward(BlocksData data, string charId, bool hasNegativceReward = true)
    {
        int positiveReward = 0;
        int negativeReward = 0;
        int flag = hasNegativceReward ? 1 : 0;
        foreach (BlockData blockData in data.Data)
        {
            if (blockData.Color == charId)
                positiveReward++;
            else if (blockData.Color != "")
                negativeReward++;
        }
        return positiveReward - negativeReward * flag;
    }

    public static BlocksData GetNewBlocksData(BlocksData currentblocksData, string charId, List<Vector2Int> path)
    {
        BlocksData blocksData = new BlocksData(currentblocksData);
        foreach (BlockData blockData in blocksData.Data)
        {
            if (path.Contains(blockData.Idx))
            {
                blockData.Color = charId;
            }
        }
        return blocksData;
    }
}
