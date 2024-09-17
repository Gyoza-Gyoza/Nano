using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgingBlock : Block
{
    public override void Activate()
    {
        Debug.Log($"{this} entered");
    }
}
