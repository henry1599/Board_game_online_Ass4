using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkInput : InputHandler
{
    private void Start()
    {
        ConnectionUtils.RegisterCallBack(DataType.MOVERMENT, CallBack);
    }

    private bool CallBack(Message msg)
    {
        var data = msg.data as PositionData;
        if (data.id == character.Id)
        {
            Vector2Int position = new Vector2Int(data.x, data.y);
            StartCoroutine(WaitGameActiveAndMove(position));
        }
        return true;
    }

    private IEnumerator WaitGameActiveAndMove(Vector2Int position)
    {
        yield return new WaitUntil(() => GameManager.IsActive);
        Block targetBlock = LevelManager.Instance.GetBlock(position);
        OnGetInput?.Invoke(character, targetBlock);
        Active = false;
    }
}
