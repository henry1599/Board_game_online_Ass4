using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
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
    private int numCharacters = 0;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
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
                blockInstance.data = this.blocksData.Data[i * 10 + j];
                this.blocks.Add(blockInstance);
            }
        }
        StartCoroutine(SetupMap());
        StartCoroutine(SetupCharacter());
    }
    IEnumerator SetupCharacter()
    {
        yield return new WaitUntil(() => this.blocksData != null);
        foreach (BlockData blockData in this.blocksData.Data)
        {
            int x = blockData.Idx.x;
            int y = blockData.Idx.y;

            string charId = blockData.CharId;
            if (charId == "") continue;

            Character charInstance = Instantiate(this.characterTemplate, this.characterContainer);
            charInstance.Id = charId;
            charInstance.CharacterColor = colors[numCharacters];
            charInstance.InputHandler.Init(charInstance);
            charInstance.InputHandler.OnGetInput += MoveCharacter;
            if (numCharacters == 0)
                charInstance.InputHandler.GetInput();
            numCharacters++;
            List<Vector2Int> movePath = charInstance.MoveToBlock(this.blocks[x * 10 + y]);
            UpdateMap(charInstance, movePath);
            this.characters.Add(charId, charInstance);
        }
    }
    IEnumerator SetupMap()
    {
        yield return new WaitUntil(() => this.blocksData != null);
        foreach (BlockData blockData in this.blocksData.Data)
        {
            int x = blockData.Idx.x;
            int y = blockData.Idx.y;

            this.blocks[x * 10 + y].SetColor(this.defaultColor);
        }
    }

    void UpdateMap(Character character, List<Vector2Int> path)
    {
        foreach (BlockData blockData in this.blocksData.Data)
        {
            if (path.Contains(blockData.Idx))
            {
                blockData.Color = character.Id;
                this.blocks[blockData.Idx.x * 10 + blockData.Idx.y].SetColor(character.CharacterColor);
            }
        }
    }

    void UpdateCharacter()
    {
        foreach (BlockData blockData in this.blocksData.Data)
        {
            int x = blockData.Idx.x;
            int y = blockData.Idx.y;

            string charId = blockData.CharId;
            if (charId != "")
            {
                Character character = this.characters[charId];
                if (character.CurrentBlock == this.blocks[x * 10 + y])
                    continue;

                List<Vector2Int> movePath = character.MoveToBlock(this.blocks[x * 10 + y]);
                UpdateMap(this.characters[charId], movePath);
            }
        }

    }
    [Button("Random characters data")]
    public void RandomData()
    {
        this.blocksData.SetRandomPositionChar();
        UpdateCharacter();
    }
    public Block GetBlock(Vector2Int idx)
    {
        return this.blocks.Find(block => block.data.Idx == idx);
    }

    public void MoveCharacter(Character character, Block block)
    {
        string currentCharId = character.Id;
        List<string> charIds = new List<string>(characters.Keys);
        int currentCharIndex = -1;
        for (int i = 0; i < charIds.Count; i++)
        {
            if (charIds[i] == currentCharId)
                currentCharIndex = i;
        }
        if (currentCharIndex == -1)
        {
            Debug.LogError($"Character id {currentCharId} not found");
            return;
        }
        else
        {
            character.CurrentBlock.data.CharId = "";
            block.data.CharId = currentCharId;
            int nextCharacterIndex = currentCharIndex + 1;
            if (nextCharacterIndex >= charIds.Count)
                nextCharacterIndex = 0;
            UpdateCharacter();
            StartCoroutine(ChangeCharacterTurn(character, charIds[nextCharacterIndex]));
        }
    }

    IEnumerator ChangeCharacterTurn(Character character, string nextCharId)
    {
        yield return null;
        yield return new WaitUntil(() => character.MoveComplete);
        characters[nextCharId].InputHandler.GetInput();
    }
}
