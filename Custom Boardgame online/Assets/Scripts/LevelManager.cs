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
    public Character agentTemplate;
    public Transform blockContainer;
    public Block blockTemplate;
    public List<Block> blocks;
    public Dictionary<string, Character> characters;
    private int numCharacters = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("--- Round : " + GameManager.CurrentRound);
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


            Character charInstance;
            if (GameManager.CurrentMode != GameMode.Training &&
                (GameManager.CurrentMode != GameMode.MachineLearning || charId == "0") &&
                (GameManager.CurrentMode != GameMode.Compare || charId == "0"))
                charInstance = Instantiate(this.characterTemplate, this.characterContainer);
            else
            {
                charInstance = Instantiate(this.agentTemplate, this.characterContainer);
                charInstance.InputHandler = charInstance.GetComponent<RLInput>();
            }
            charInstance.Id = charId;
            charInstance.CharacterColor = colors[int.Parse(charId)];
            if (GameManager.CurrentMode == GameMode.Online)
            {
                if (charId == GameManager.PlayerId)
                    charInstance.InputHandler = charInstance.gameObject.AddComponent<MouseInput>();
                else
                    charInstance.InputHandler = charInstance.gameObject.AddComponent<NetworkInput>();
            }
            else if (GameManager.CurrentMode == GameMode.Compare)
            {
                if (numCharacters == 0)
                {
                    charInstance.InputHandler = charInstance.gameObject.AddComponent<RandomInput>();
                }
            }
            else
            {
                if (GameManager.CurrentMode != GameMode.Training && numCharacters == 0)
                {
                    charInstance.InputHandler = charInstance.gameObject.AddComponent<MouseInput>();
                }
                else
                {
                    if (GameManager.CurrentMode == GameMode.Minimax)
                        charInstance.InputHandler = charInstance.gameObject.AddComponent<MinimaxInput>();
                    else if (GameManager.CurrentMode == GameMode.Random)
                        charInstance.InputHandler = charInstance.gameObject.AddComponent<RandomInput>();
                }
            }
            charInstance.InputHandler.Init(charInstance);
            charInstance.InputHandler.OnGetInput += MoveCharacter;
            if (numCharacters == 0)
                charInstance.InputHandler.GetInput();
            charInstance.Init(int.Parse(charInstance.Id));

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
            UpdateCharacter();
            Character nextCharacter = GetNextCharacter(currentCharId);
            if (nextCharacter != null)
                StartCoroutine(ChangeCharacterTurn(character, nextCharacter.Id));
        }
    }

    IEnumerator ChangeCharacterTurn(Character character, string nextCharId)
    {
        yield return null;
        yield return new WaitUntil(() => character.MoveComplete);
        characters[nextCharId].InputHandler.GetInput();
        // Debug.Log($"Score: {Utils.GetReward(blocksData, "0", false)}, {Utils.GetReward(blocksData, "1", false)}; Turn: {nextCharId}");
    }

    public Character GetNextCharacter(string currentCharId, bool isMinimax = false)
    {
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
            return null;
        }

        int nextCharacterIndex = currentCharIndex + 1;
        if (nextCharacterIndex >= charIds.Count)
        {
            if (!isMinimax)
                GameManager.CurrentRound++;
            if (GameManager.CurrentRound > GameManager.Round)
            {
                if (GameManager.CurrentMode == GameMode.Training)
                    GameManager.ResetGame();
                else
                    GameManager.EndGame();
                return null;
            }
            else
                Debug.Log("--- Round : " + GameManager.CurrentRound);
            nextCharacterIndex = 0;
        }

        return characters[charIds[nextCharacterIndex]];
    }
}
