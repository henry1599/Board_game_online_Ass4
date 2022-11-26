using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using UnityEngine.SceneManagement;

public class VictoryScreenManager : MonoBehaviour
{
    public static VictoryScreenManager Instance {get; private set;}
    private static readonly int InTransitionKey = Animator.StringToHash("in");
    private static readonly int[] DanceTransitionKeys = new int[]
    {
        Animator.StringToHash("Dance01"),
        Animator.StringToHash("Dance02"),
        Animator.StringToHash("Dance03"),
        Animator.StringToHash("Dance04")
    };
    [SerializeField] Button backButton;
    [SerializeField] Button restartButton;
    [SerializeField] GameObject container;
    [SerializeField] GameObject character;
    [SerializeField] List<GameObject> bodies;
    [SerializeField] RuntimeAnimatorController characterAnim;
    [SerializeField] Animator transitionAnim;
    [SerializeField] GameObject normalCamera;
    public System.Action OnExit;
    public System.Action OnRestart;
    void Awake()
    {
        Instance = this;
    }
    void OnDestroy()
    {
        RemoveAllListeners(ref OnExit);
        RemoveAllListeners(ref OnRestart);
        Instance = null;
    }
    private void RemoveAllListeners(ref System.Action action)
    {
        if (action == null) return;

        var listeners = action.GetInvocationList();
        foreach (var l in listeners)
        {
            action -= (System.Action)l;
        }
    }
    [NaughtyAttributes.Button("Show Victory Screen")]
    public void Show(int charId)
    {
        StartCoroutine(Cor_Show(charId));

        backButton.onClick.AddListener(() => OnBackButtonClick());
        restartButton.onClick.AddListener(() => OnRestartButtonClick());
    }
    IEnumerator Cor_Show(int charId)
    {
        this.container.gameObject.SetActive(true);
        // SoundManager.Instance.PlaySound(SoundID.TRANSITION_IN);
        this.transitionAnim.CrossFade(InTransitionKey, 0, 0);
        this.normalCamera.SetActive(true);

        yield return new WaitForSeconds(1f);
        // SoundManager.Instance.PlaySound(SoundID.CONFETTI);
        // SoundManager.Instance.PlaySound(SoundID.WIN);
        CreateCharacter(charId);
        yield return new WaitForSeconds(0.5f);
    }
    void CreateCharacter(int charId)
    {
        foreach (var body in bodies)
        {
            body.SetActive(false);
        }
        bodies[charId].SetActive(true);
        this.character.SetActive(true);
        this.character.GetComponentInChildren<Animator>().runtimeAnimatorController = this.characterAnim;
        this.character.GetComponentInChildren<Animator>().CrossFade(DanceTransitionKeys[Random.Range(0, DanceTransitionKeys.Length)], 0, 0);
    }
    public void OnBackButtonClick()
    {
        UIUtils.LockInput();
        GameSceneManager.Instance.TransitionIn(() => 
            {
                UIUtils.UnlockInput();
                GameEvents.LOAD_SCENE?.Invoke("Main");
            }
        );
    }
    public void OnRestartButtonClick()
    {
        UIUtils.LockInput();
        GameSceneManager.Instance.TransitionIn(() => 
            {
                UIUtils.UnlockInput();
                GameEvents.LOAD_SCENE?.Invoke("Gameplay");
            }
        );
    }
}
