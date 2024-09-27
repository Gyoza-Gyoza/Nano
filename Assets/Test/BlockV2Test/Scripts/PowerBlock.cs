using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBlock : Block
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isDraining = true;
            Activate();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isDraining = false;
            Deactivate();
        }
    }
    public override void Activate()
    {
        foreach(Block block in neighbours)
        {
            block.Charge();
        }
    }
    public override void Deactivate()
    {
        foreach (Block block in neighbours)
        {
            if (block is BridgingBlock) continue;
            block.Discharge();
        }
    }
}
