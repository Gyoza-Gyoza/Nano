using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBlock : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public bool playerOnTiles = false;

    private BoxCollider2D col;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
    }

    public void ActivateTile(Color colorToChange)
    {
        spriteRenderer.color = colorToChange;
        col.isTrigger = false;
    }
    public void DeactivateTile(Color colorToChange)
    {
        spriteRenderer.color = colorToChange; 
        col.isTrigger = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerOnTiles = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        playerOnTiles = false;
    }
}
