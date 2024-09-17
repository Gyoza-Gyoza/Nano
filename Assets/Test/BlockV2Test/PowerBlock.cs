using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBlock : Block
{
    private HashSet<Block> activatables = new HashSet<Block>();
    public HashSet<Block> Activatables
    { 
        get { return activatables; }
        set { activatables = value; }
    }
    public void AddActivatables(Block activatableToAdd)
    {
        if(!activatables.Contains(activatableToAdd))
        {
            activatables.Add(activatableToAdd);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            foreach(Block activatableBlocks in activatables)
            {
                
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
    private void Activate()
    {
        foreach(Block block in activatables)
        {
            switch (block)
            {
                case PlatformBlock platformBlock:
                    platformBlock.Activate();
                    break;

                case BridgingBlock bridgingBlock:
                    bridgingBlock.Activate();
                    break;

                case MissileBlock missileBlock:
                    missileBlock.Activate();
                    break;

                default:
                    Activate();
                    break;
            }
        }
    }
}
