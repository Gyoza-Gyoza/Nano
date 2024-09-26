using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBlock : Block
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Activate();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
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
            block.Discharge();
        }
    }
}
