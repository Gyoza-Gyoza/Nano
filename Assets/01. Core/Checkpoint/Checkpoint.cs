using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] protected Sprite activeSprite, inactiveSprite;
    [SerializeField] protected Material activeMaterial, inactiveMaterial;
    private static Checkpoint currentCheckpoint;
    public Transform respawnPoint;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Collider2D col;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
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

            spriteRenderer.material = activeMaterial;
            spriteRenderer.sprite = activeSprite;
            col.enabled = false;
            currentCheckpoint = this;
            animator.SetBool("isActive", true);
        }
    }

    private void DeactivateCheckpoint()
    {
        spriteRenderer.material = inactiveMaterial;
        spriteRenderer.sprite = inactiveSprite;
        col.enabled = true;
        animator.SetBool("isActive", false);
    }
}
