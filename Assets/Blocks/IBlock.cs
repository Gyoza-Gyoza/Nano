using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlock
{
    public IEnumerator Activate(HashSet<BlockBehaviour> passedBlocks, WaitForSeconds timeBetweenActivations);
    public IEnumerator Deactivate(HashSet<BlockBehaviour> passedBlocks, WaitForSeconds timeBetweenDeactivations);
}

public enum BlockState
{
    Inactive, 
    Active, 
    Power
}