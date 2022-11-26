using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System;

public class RLAgent : Agent
{
    private Dictionary<string, int> ColorMapping;
    private Character character;
    private Action<Vector2Int> OnReceiveAction;
    private bool agentActive;
    private int lastReward = 0;
    public void Init(Character character, Action<Vector2Int> onReceiveAction)
    {
        this.character = character;
        this.OnReceiveAction = onReceiveAction;
        ColorMapping = new Dictionary<string, int>();
        ColorMapping.Add("", -1);
        ColorMapping.Add("0", 0);
        ColorMapping.Add("1", 1);
        MaxStep = GameManager.Round;
    }
    public override void OnEpisodeBegin()
    {
        agentActive = true;
        base.OnEpisodeBegin();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                sensor.AddObservation(ColorMapping[LevelManager.Instance.blocksData.Data[x * 10 + y].Color]);
            }
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (!agentActive)
            return;

        int x = actions.DiscreteActions[0] / 10;
        int y = actions.DiscreteActions[0] % 10;
        Vector2Int targetBlock = new Vector2Int(x, y);
        if (IsValid(targetBlock))
        {
            OnReceiveAction?.Invoke(targetBlock);
            int reward = Utils.GetReward(LevelManager.Instance.blocksData, character.Id, true);
            SetReward(reward - lastReward);
            lastReward = reward;
            if (GameManager.CurrentRound >= GameManager.Round && agentActive)
            {
                agentActive = false;
                EndEpisode();
            }
        }
        else
        {
            SetReward(-1000);
            RequestDecision();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        List<Vector2Int> moveableBlocks = character.MoveableBlocks;
        Vector2Int targetBlock = moveableBlocks[UnityEngine.Random.Range(0, moveableBlocks.Count)];
        var discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = targetBlock.x * 10 + targetBlock.y;
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        Debug.Log("---------");
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                bool isValid = IsValid(new Vector2Int(x, y));

                actionMask.SetActionEnabled(0, x * 10 + y, isValid);
            }
        }
    }

    private bool IsValid(Vector2Int blockIndex)
    {
        return character.MoveableBlocks.Contains(blockIndex);
    }
}
