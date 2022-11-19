using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Character : MonoBehaviour
{
    public InputHandler InputHandler;
    public Animator animator;
    public string Id;
    public Block CurrentBlock;
    public List<Vector2Int> MoveableBlocks;
    public Color CharacterColor;
    public bool MoveComplete = false;

    public List<Vector2Int> MoveToBlock(Block block)
    {
        MoveComplete = false;
        List<Vector2Int> movePath;
        if (CurrentBlock == null)
        {
            movePath = new List<Vector2Int>();
            movePath.Add(new Vector2Int(block.data.Idx.x, block.data.Idx.y));
            transform.localPosition = block.transform.localPosition;
        }
        else
        {
            Quaternion rotation = Quaternion.LookRotation(block.transform.position - transform.position, Vector3.up);
            float moveDuration = Mathf.Clamp((block.data.Idx - CurrentBlock.data.Idx).magnitude / 2, 0.5f, 2);
            movePath = Utils.GetMovePath(CurrentBlock, block);
            animator.SetBool("IsRunning", true);
            transform.DOLocalMove(block.transform.localPosition, moveDuration)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        MoveComplete = true;
                        animator.SetBool("IsRunning", false);
                    });
            transform.DORotateQuaternion(rotation, 0.5f).SetEase(Ease.Linear);
        }

        CurrentBlock = block;
        MoveableBlocks = Utils.GetMoveableBlocks(CurrentBlock.data);
        return movePath;
    }

    private void OnDrawGizmos()
    {
        if (CurrentBlock != null)
        {
            foreach (Vector2Int idx in MoveableBlocks)
            {
                Block block = LevelManager.Instance.GetBlock(idx);
                Gizmos.color = CharacterColor;
                Gizmos.DrawSphere(block.transform.position + Vector3.up * Random.Range(0.2f, 0.5f), 0.1f);
            }
        }
        if (InputHandler != null && InputHandler.Active)
        {
            Gizmos.color = CharacterColor;
            Gizmos.DrawSphere(transform.position + Vector3.up * 2, 0.5f);
        }
    }
}
