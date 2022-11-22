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
        };
    }

    private bool IsValid(Block block)
    {
        return character.MoveableBlocks.Contains(block.data.Idx);
    }
}
