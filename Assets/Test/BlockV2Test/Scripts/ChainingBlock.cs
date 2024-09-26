using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChainingBlock : Block
{
    public override IEnumerator ActivateBlock()
    {
        yield return delay;

        TurnOn();
    }
    public override IEnumerator DeactivateBlock()
    {
        yield return delay;

        TurnOff();
    }

    protected virtual void TurnOn() //Turning on functionalities here
    {
        spriteRenderer.color = activeColor;
        col.enabled = true;
    }
    protected virtual void TurnOff() //Turning off functionalities here
    {
        spriteRenderer.color = inactiveColor;
        col.enabled = false;
    }
}
