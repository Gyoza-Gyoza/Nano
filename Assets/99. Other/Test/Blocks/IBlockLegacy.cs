using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlockLegacy 
{
    public void UpdateBlockState();
    public IEnumerator ChangeBlockState(BlockStateLegacy newState, WaitForSeconds timeToWait);
}

public enum BlockStateLegacy
{
    Inactive,
    Active, 
    Start
}