using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputHandler : MonoBehaviour
{
    protected Character character;
    public bool Active { get; protected set; } = false;
    public Action<Character, Block> OnGetInput;

    public virtual void Update()
    {
        character.arrow.SetActive(Active);
    }

    public virtual void Init(Character character)
    {
        this.character = character;
    }

    public virtual void GetInput()
    {
        Active = true;
    }
}
