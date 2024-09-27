using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChainingBlock : Block
{
    [SerializeField]
    protected float chainTime;

    protected WaitForSeconds delay;
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
        spriteRenderer.material = activeMaterial;
        spriteRenderer.sprite = activeSprite;
        col.enabled = true;
    }
    protected virtual void TurnOff() //Turning off functionalities here
    {
        spriteRenderer.material = inactiveMaterial;
        spriteRenderer.sprite = inactiveSprite;
        col.enabled = false;
    }
}
