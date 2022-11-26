using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouseInput : InputHandler
{
    private void Start()
    {
        Block.OnClick += OnClickBlock;
    }

    private void OnDestroy()
    {
        Block.OnClick -= OnClickBlock;
    }

    private bool IsValid(Block block)
    {
        return character.MoveableBlocks.Contains(block.data.Idx);
    }

    private void OnClickBlock(Block block)
    {
        if (!Active || !GameManager.IsActive)
            return;

        if (!IsValid(block))
            return;

        Debug.Log(character.Id);
        OnGetInput?.Invoke(character, block);
        Active = false;

        if (GameManager.CurrentMode == GameMode.Online)
        {
            var pos = new PositionData();
            pos.id = character.Id;
            pos.x = block.data.Idx.x;
            pos.y = block.data.Idx.y;
            var msg = new Message();
            msg.type = DataType.MOVERMENT;
            msg.data = pos;
            ConnectionUtils.SendMessage(msg);
        }
    }
}
