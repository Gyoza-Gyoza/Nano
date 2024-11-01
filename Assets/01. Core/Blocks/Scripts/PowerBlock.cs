using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PowerBlock : Block
{
    [SerializeField]
    private ParticleSystem lightningVFX; 
    private Animator lightningAnim;
    [SerializeField]
    private int numberOfAnimations;
    private bool activated = false;
    private BoxCollider2D col;

    [SerializeField]
    private Vector2 yRandomPos, animationRotationRange; 

    private void Awake()
    {
        lightningAnim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        lightningVFX = GetComponentInChildren<ParticleSystem>();
    }
    private void Start()
    {
        connections.Add(this, false);
        foreach (Block block in neighbours) //Checks through neighbours 
        {
            block.InitializeConnections(this, connections);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            activated = true;
            PlayerOnBlock(this, true);
            Activate();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            activated = false;
            PlayerOnBlock(this, false);
            Deactivate();
        }
    }
    public override void Activate()
    {
        lightningVFX.Play();
        foreach(Block block in neighbours)
        {
            block.StartCoroutine(block.Charge(new HashSet<Block>()));
        }
    }
    public override void Deactivate()
    {
        lightningVFX.Stop();
        foreach (Block block in neighbours)
        {
            if (block is BridgingBlock) continue;
            block.StartCoroutine(block.Discharge(new HashSet<Block>()));
        }
    }
}
