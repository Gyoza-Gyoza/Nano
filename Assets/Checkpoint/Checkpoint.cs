using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private static Checkpoint currentCheckpoint;
    public Transform respawnPoint;

    SpriteRenderer spriteRenderer;
    public Sprite passive, active;

    public Collider2D col;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            ActivateCheckpoint();
            PlayerBehaviour.player.UpdateRespawnPoint(respawnPoint.position);
        }
    }

    private void ActivateCheckpoint()
    {
        //If this checkpoint is not the currently active one
        if(currentCheckpoint != this)
        {
            //Deactivate the previous checkpoint
            if(currentCheckpoint != null)
            {
                currentCheckpoint.DeactivateCheckpoint();
            }

            spriteRenderer.sprite = active;
            col.enabled = false;
            currentCheckpoint = this;
        }
    }

    private void DeactivateCheckpoint()
    {
        spriteRenderer.sprite = passive;
        col.enabled = true;
    }
}
