using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance {get; private set;}
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
        GameEvents.LOAD_SCENE += HandleLoadScene;
    }
    void OnDestroy()
    {
        GameEvents.LOAD_SCENE -= HandleLoadScene;
    }
    void HandleLoadScene(string sceneName)
    {
        LoadSceneByID(sceneName);
    }
    void LoadSceneByID(string sceneName)
    {
        UIUtils.LockInput();
        TransitionIn(() => 
        {
            SceneManager.LoadScene(sceneName);
            TransitionOut(() => 
            {
                UIUtils.UnlockInput();
            });
        });
    }
    
    public void TransitionIn(System.Action _cb = null)
    {
        StartCoroutine(Cor_TransitionIn(_cb));
    }
    public void TransitionOut(System.Action _cb = null)
    {
        StartCoroutine(Cor_TransitionOut(_cb));
    }
    IEnumerator Cor_TransitionIn(System.Action _cb = null)
    {
        yield return new WaitUntil(() => SceneTransition.Instance != null);
        SceneTransition.Instance.TransitionIn();
        yield return new WaitForSeconds(GameConstants.TRANSITION_IN_DURATION);
        _cb?.Invoke();
    }
    IEnumerator Cor_TransitionOut(System.Action _cb = null)
    {
        yield return new WaitUntil(() => SceneTransition.Instance != null);
        if (GameManager.CurrentMode == GameMode.MachineLearning ||
            GameManager.CurrentMode == GameMode.Compare)
            yield return new WaitForSeconds(GameConstants.TRANSITION_IN_ML_DELAY);
        SceneTransition.Instance.TransitionOut();
        yield return new WaitForSeconds(GameConstants.TRANSITION_OUT_DURATION);
        _cb?.Invoke();
    }
}
