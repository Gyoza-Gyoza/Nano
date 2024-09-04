using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlock 
{
    public void UpdateBlockState();
    public IEnumerator ChangeBlockState(BlockState newState, WaitForSeconds timeToWait);
}

public enum BlockState
{
    Inactive,
    Active, 
    Start
}