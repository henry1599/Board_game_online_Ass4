using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum GameMode
{
    Random,
    Minimax,
    MachineLearning,
    Online,
    Training,
    Compare,
}

public enum MinimaxMode
{
    Easy,
    Normal,
    Hard,
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManagerPrefab;
    [SerializeField] private GameObject waitingCanvas;
    public static GameMode CurrentMode = GameMode.Random;
    public static MinimaxMode MinimaxCurrentMode = MinimaxMode.Easy;
    public static string PlayerId = "0";
    public RoundConfig RoundConfig;
    public static int Round {get; set;}
    public static int CurrentRound
    {
        get => currentRound;
        set
        {
            currentRound = value;
            ON_ROUND_CHANGED?.Invoke(Round - value + 1);
        }
    } private static int currentRound = 1;
    private LevelManager currentLevelManager = null;
    public static System.Action<int> ON_ROUND_CHANGED;

    private static bool _isActive = true;
    private static List<bool> _playerActive = new List<bool>() { false, false };
    public static bool IsActive
    {
        get
        {
            if (CurrentMode == GameMode.Online)
            {
                return _playerActive[0] && _playerActive[1];
            }
            else
            {
                return _isActive;
            }
        }
    }

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            ConnectionUtils.RegisterCallBack(DataType.ACTIVE_STATUS, ListenPlayerActive);
            if (CurrentMode == GameMode.Training)
            {
                StartGame(CurrentMode);
            }
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Round = RoundConfig.Round;
    }
    private void Update()
    {
        if (currentLevelManager == null)
            waitingCanvas.SetActive(false);
        else
            waitingCanvas.SetActive(_isActive != IsActive);

        if (Input.GetKeyDown(KeyCode.Escape) && currentLevelManager != null)
        {
            GameEvents.PAUSE?.Invoke(_isActive);
            _isActive = !_isActive;
            Debug.Log("Game active : " + _isActive);
            if (CurrentMode == GameMode.Online)
            {
                var activeData = new ActiveData();
                activeData.id = PlayerId;
                activeData.active = !_playerActive[int.Parse(PlayerId)];
                var msg = new Message();
                msg.type = DataType.ACTIVE_STATUS;
                msg.data = activeData;
                ConnectionUtils.SendMessage(msg);
            }
        }
    }
    public static void StartGame(GameMode mode)
    {
        CurrentMode = mode;
        ResetGame();
    }
    public static void ResetGame()
    {
        _isActive = false;
        if (CurrentMode == GameMode.Online)
        {
            var activeData = new ActiveData();
            activeData.id = PlayerId;
            activeData.active = false;
            var msg = new Message();
            msg.type = DataType.ACTIVE_STATUS;
            msg.data = activeData;
            ConnectionUtils.SendMessage(msg);
        }
        CurrentRound = 1;
        instance.ResetLevel();
    }

    public void ResetLevel()
    {
        StartCoroutine(ResetCoroutine());
    }

    public IEnumerator ResetCoroutine()
    {
        yield return new WaitForSeconds(2);
        var currentLevel = FindObjectOfType<LevelManager>();
        if (currentLevel != null)
            Destroy(currentLevel.gameObject);
        currentLevelManager = Instantiate(levelManagerPrefab, Vector3.zero, Quaternion.identity);
        _isActive = true;
        if (CurrentMode == GameMode.Online)
        {
            var activeData = new ActiveData();
            activeData.id = PlayerId;
            activeData.active = true;
            var msg = new Message();
            msg.type = DataType.ACTIVE_STATUS;
            msg.data = activeData;
            ConnectionUtils.SendMessage(msg);
        }
    }

    public static void SetPlayerActive(bool isActive, int index)
    {
        _playerActive[index] = isActive;
        _isActive = isActive;
    }

    private bool ListenPlayerActive(Message msg)
    {
        var data = msg.data as ActiveData;
        _playerActive[int.Parse(data.id)] = data.active;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat("player 0: {0}, player 1: {1}", _playerActive[0], _playerActive[1]);
        Debug.Log(stringBuilder.ToString());
        return true;
    }

    public static void EndGame()
    {
        _isActive = false;
        string playerWin = "";
        int maxScore = 0;
        StringBuilder stringBuilder = new StringBuilder("Final Score \n");
        Dictionary<string, int> finalScore = new Dictionary<string, int>();
        foreach (var kv in LevelManager.Instance.characters)
        {
            finalScore.Add(kv.Key, Utils.GetReward(LevelManager.Instance.blocksData, kv.Key, false));
            stringBuilder.AppendFormat("{0}: {1}\n", kv.Key, finalScore[kv.Key]);
            if (finalScore[kv.Key] > maxScore)
            {
                maxScore = finalScore[kv.Key];
                playerWin = kv.Key;
            }
        }
        Debug.Log(stringBuilder.ToString());
        Debug.Log("Player win: " + playerWin);
        instance.DelayShowVictory(int.Parse(playerWin));
    }

    public void DelayShowVictory(int charId)
    {
        StartCoroutine(DelayShowVictoryCoroutine(charId));
    }

    public IEnumerator DelayShowVictoryCoroutine(int charId)
    {
        yield return new WaitForSeconds(2);
        VictoryScreenManager.Instance.Show(charId);
    }
}
