using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Character : MonoBehaviour
{
    private static readonly int MoveKeyAnimation = Animator.StringToHash("Move");
    private static readonly int IdleKeyAnimation = Animator.StringToHash("Idle");
    public InputHandler InputHandler;
    public string Id {get; set;}
    public Block CurrentBlock {get; set;}
    public List<Vector2Int> MoveableBlocks {get; set;}
    public Color CharacterColor {get; set;}
    public bool MoveComplete
    {
        get => this.moveComplete;
        set => this.moveComplete = value;
    } bool moveComplete;
    public Animator Animator;
    void Start()
    {
        this.Animator.CrossFade(IdleKeyAnimation, 0, 0);    
    }
    public List<Vector2Int> MoveToBlock(Block block)
    {
        this.Animator.CrossFade(MoveKeyAnimation, 0, 0);
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
            float moveDuration = Mathf.Clamp((block.data.Idx - CurrentBlock.data.Idx).magnitude, 0.5f, 2);
            movePath = Utils.GetMovePath(CurrentBlock, block);
            transform.DOLocalMove(block.transform.localPosition, moveDuration)
                    .SetEase(Ease.Linear).OnComplete(() => 
                    {
                        MoveComplete = true;
                        this.Animator.CrossFade(IdleKeyAnimation, 0, 0);
                    });
            transform.DORotateQuaternion(rotation, 0.25f).SetEase(Ease.Linear);
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
