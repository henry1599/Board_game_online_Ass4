using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlocksData
{
    public List<BlockData> Data; 
    public BlocksData()
    {
        this.Data = new List<BlockData>();
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                BlockData blockData = new BlockData(new Vector2Int(x, y), new float[4] {1, 1, 1, 1}, string.Empty);
                this.Data.Add(blockData);
            }
        }
        this.Data[0 * 10 + 0].CharId = "0";
        this.Data[0 * 10 + 0].CharRotation = -90;

        this.Data[9 * 10 + 0].CharId = "1";
        this.Data[9 * 10 + 0].CharRotation = 90;

        this.Data[0 * 10 + 9].CharId = "2";
        this.Data[0 * 10 + 9].CharRotation = 180;

        this.Data[9 * 10 + 9].CharId = "3";
        this.Data[9 * 10 + 9].CharRotation = 180;
    }
    public void SetRandomPositionChar()
    {
        foreach (BlockData data in this.Data)
        {
            data.CharId = "";
        }
        List<Vector2Int> randIdxChars = new List<Vector2Int>()
        {
            new Vector2Int(Random.Range(0, 10), Random.Range(0, 10)),
            new Vector2Int(Random.Range(0, 10), Random.Range(0, 10)),
            new Vector2Int(Random.Range(0, 10), Random.Range(0, 10)),
            new Vector2Int(Random.Range(0, 10), Random.Range(0, 10))
        };
        this.Data[randIdxChars[0].x * 10 + randIdxChars[0].y].CharId = "0";
        this.Data[randIdxChars[1].x * 10 + randIdxChars[1].y].CharId = "1";
        this.Data[randIdxChars[2].x * 10 + randIdxChars[2].y].CharId = "2";
        this.Data[randIdxChars[3].x * 10 + randIdxChars[3].y].CharId = "3";
    }
}
[System.Serializable]
public class BlockData
{
    public Vector2Int Idx;
    public float[] Color; // * r b g a
    public string CharId;
    public float CharRotation;
    public BlockData()
    {
        Color = new float[4] {1, 1, 1, 1};
        CharId = string.Empty;
    }
    public BlockData(Vector2Int idx, float[] color, string charId = "")
    {
        this.Idx = idx;
        this.Color = color;
        this.CharId = charId;
    }
}
