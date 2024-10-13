using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class BridgingBlock : ChainingBlock
{
    private PowerBlock managerBlock;

    private bool playerCollided = false,
        bridgeActivated = false;

    //public override bool IsCharged
    //{
    //    get { return isCharged; }
    //    set
    //    {
    //        isCharged = value;

    //        if (IsCharged) StartCoroutine(ActivateBlock());
    //        else StartCoroutine(DeactivateBlock());
    //    }
    //}

    private void Start()
    {
        /*if (this is BridgingBlock)*/ col.enabled = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") managerBlock.PlayerOnBlock(this, true);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") managerBlock.PlayerOnBlock(this, false);
    }
    public void InitializeConnections(PowerBlock managerBlock, Dictionary<Block, bool> connectionList)
    {
        if (connectionList.ContainsKey(this)) return; //If dictionary contains the current block, exit function

        connectionList.Add(this, false);
        this.managerBlock = managerBlock;


        foreach (Block block in neighbours)
        {
            if (block is BridgingBlock)
            {
                BridgingBlock bridgingBlock = block as BridgingBlock;
                bridgingBlock.InitializeConnections(managerBlock, connectionList); //Continues to the next block 
            }
        }
    }
    protected override void TurnOn()
    {
        spriteRenderer.material = activeMaterial;
        spriteRenderer.sprite = activeSprite;
        col.enabled = true;
    }
    protected override void TurnOff()
    {
        spriteRenderer.material = inactiveMaterial;
        spriteRenderer.sprite = inactiveSprite;
        col.enabled = false;
    }
}
