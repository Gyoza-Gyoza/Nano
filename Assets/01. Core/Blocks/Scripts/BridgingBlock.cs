using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class BridgingBlock : ChainingBlock
{
    private void Start()
    {
        col.isTrigger = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") managerBlock.PlayerOnBlock(this, true);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") managerBlock.PlayerOnBlock(this, false);
    }
    protected override void TurnOn()
    {
        spriteRenderer.material = activeMaterial;
        spriteRenderer.sprite = activeSprite;
        col.isTrigger = false;
        transform.gameObject.tag = "Charged Ground";
    }
    protected override void TurnOff()
    {
        spriteRenderer.material = inactiveMaterial;
        spriteRenderer.sprite = inactiveSprite;
        col.isTrigger = true;
        transform.gameObject.tag = "Uncharged Block";
    }
}
