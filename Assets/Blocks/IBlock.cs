using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlock
{
    public void Activate(HashSet<BlockBehaviour> passedBlocks);
    public void Deactivate(HashSet<BlockBehaviour> passedBlocks);
}

public enum BlockState
{
    Inactive, 
    Active, 
    Power
}