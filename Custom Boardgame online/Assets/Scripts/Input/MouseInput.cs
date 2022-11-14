using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouseInput : InputHandler
{
    private void Start()
    {
        Block.OnClick += (Block block) =>
        {
            if (!Active)
                return;

            if (!IsValid(block))
                return;

            Debug.Log(character.Id);
            OnGetInput?.Invoke(character, block);
            Active = false;
        };
    }

    private bool IsValid(Block block)
    {
        return character.MoveableBlocks.Contains(block.data.Idx);
    }
}
