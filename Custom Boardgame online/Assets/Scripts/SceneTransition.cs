using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public static readonly int InKeyAnimation = Animator.StringToHash("in");
    public static readonly int OutKeyAnimation = Animator.StringToHash("out");
    public static SceneTransition Instance {get; private set;}
    private Animator m_Animator;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        m_Animator = GetComponent<Animator>();
    }
    public void TransitionIn()
    {
        m_Animator.CrossFade(InKeyAnimation, 0, 0);
    }
    public void TransitionOut()
    {
        m_Animator.CrossFade(OutKeyAnimation, 0, 0);
    }
}
