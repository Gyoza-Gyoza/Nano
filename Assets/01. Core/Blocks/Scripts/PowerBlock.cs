using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PowerBlock : Block
{
    [SerializeField]
    private GameObject lightningVFX; 
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
    }
    private void Start()
    {
        connections.Add(this, false);
        foreach (Block block in neighbours) //Checks through neighbours 
        {
            block.InitializeConnections(this, connections);
        }
        lightningVFX.transform.localScale = new Vector3(
            lightningVFX.transform.localScale.x / transform.localScale.x,
            lightningVFX.transform.localScale.y / transform.localScale.y,
            lightningVFX.transform.localScale.z / transform.localScale.z);
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
        SwitchAnimation();
        foreach(Block block in neighbours)
        {
            block.StartCoroutine(block.Charge(new HashSet<Block>()));
        }
    }
    public override void Deactivate()
    {
        foreach (Block block in neighbours)
        {
            if (block is BridgingBlock) continue;
            block.StartCoroutine(block.Discharge(new HashSet<Block>()));
        }
    }   
    public void SwitchAnimation()
    {
        if (!activated)
        {
            lightningAnim.SetInteger("State", 0);
            return;
        }
        lightningVFX.transform.position = new Vector3(
            Random.Range(col.bounds.min.x, col.bounds.max.x),
            Random.Range(transform.position.y + yRandomPos.x, transform.position.y + yRandomPos.y),
            Random.Range(col.bounds.min.z, col.bounds.max.z));
        lightningVFX.transform.Rotate(new Vector3(0f, 0f, Random.Range(animationRotationRange.x, animationRotationRange.y)));
        lightningAnim.SetInteger("State", Random.Range(1, numberOfAnimations+1));
    }
}
