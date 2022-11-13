using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class LevelManager : MonoBehaviour
{
    public Color defaultColor;
    public Color[] colors;
    public Vector2[] charIdxs;
    public BlocksData blocksData = null;
    public Transform characterContainer;
    public Character characterTemplate;
    public Transform blockContainer;
    public Block blockTemplate;
    public List<Block> blocks;
    Dictionary<string, Character> characters;
    // Start is called before the first frame update
    void Start()
    {
        this.blocksData = new BlocksData();
        this.characters = new Dictionary<string, Character>();
        SpawnMap();
    }
    void SpawnMap()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Vector3 localPosition = new Vector3(2 - i, 0, -2 + j);
                Block blockInstance = Instantiate(this.blockTemplate, this.blockContainer);
                blockInstance.transform.localPosition = localPosition;
                this.blocks.Add(blockInstance);
            }
        }
        UpdateMap();
        UpdateCharacter(false);
    }
    void UpdateMap()
    {
        StartCoroutine(Cor_UpdateMap());
    }
    void UpdateCharacter(bool isSetup)
    {
        StartCoroutine(Cor_UpdateCharacter(isSetup));
    }
    IEnumerator Cor_UpdateMap()
    {
        yield return new WaitUntil(() => this.blocksData != null);
        foreach (BlockData blockData in this.blocksData.Data)
        {
            int x = blockData.Idx.x;
            int y = blockData.Idx.y;

            this.blocks[x * 10 + y].SetColor(this.defaultColor);
        }
    }
    IEnumerator Cor_UpdateCharacter(bool isSetup)
    {
        yield return new WaitUntil(() => this.blocksData != null);
        foreach (BlockData blockData in this.blocksData.Data)
        {
            int x = blockData.Idx.x;
            int y = blockData.Idx.y;

            string charId = blockData.CharId;
            if (charId == "") continue;

            if (isSetup)
            {
                this.characters[charId].transform.localPosition = this.blocks[x * 10 + y].transform.localPosition;
            }
            else
            {
                Character charInstance = Instantiate(this.characterTemplate, this.characterContainer);
                charInstance.transform.localPosition = this.blocks[x * 10 + y].transform.localPosition;
                charInstance.transform.localEulerAngles = new Vector3(0, blockData.CharRotation, 0);
                charInstance.Id = charId;
                this.characters.TryAdd(charId, charInstance);
            }
        }

    }
    [Button("Random characters data")]
    public void RandomData()
    {
        this.blocksData.SetRandomPositionChar();
        UpdateCharacter(true);
    }
}
