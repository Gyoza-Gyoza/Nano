using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PowerBlock : Block
{
    private void Start()
    {
        connections.Add(this, false);
        foreach (Block block in neighbours) //Checks through neighbours 
        {
            block.InitializeConnections(this, connections);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerOnBlock(this, true);
            Activate();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerOnBlock(this, false);
            Deactivate();
        }
    }
    public override void Activate()
    {
        foreach(Block block in neighbours)
        {
            block.StartCoroutine(block.Charge(new HashSet<Block>()));
        }
    }
    public override void Deactivate()
    {
        foreach (Block block in neighbours)
        {
            if (block is BridgingBlock) continue;
            block.StartCoroutine(block.Discharge(new HashSet<Block>()));
        }
    }
}