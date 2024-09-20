using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChainingBlock : Block
{
    [SerializeField]
    protected Color activeColor,
        inactiveColor;

    //Keeps a list of neighbouring blocks 
    protected HashSet<Block> neighbours = new HashSet<Block>();

    protected BoxCollider2D col;

    protected SpriteRenderer spriteRenderer;

    protected WaitForSeconds delay;

    [SerializeField]
    protected float chainTime;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        InitializeNeighbours(); //Initialize the neighbouring blocks for each block
    }

    protected virtual void InitializeNeighbours() //Used to initialize the neighbouring blocks for each block
    {
        List<RaycastHit2D[]> hits = new List<RaycastHit2D[]>() //Contains everything that are beside each block
        {
            Physics2D.BoxCastAll(transform.position, transform.localScale * 0.9f, 0f, Vector2.right, col.bounds.extents.x + 0.1f),
            Physics2D.BoxCastAll(transform.position, transform.localScale * 0.9f, 0f, Vector2.left, col.bounds.extents.x + 0.1f),
            Physics2D.BoxCastAll(transform.position, transform.localScale * 0.9f, 0f, Vector2.up, col.bounds.extents.y + 0.1f),
            Physics2D.BoxCastAll(transform.position, transform.localScale * 0.9f, 0f, Vector2.down, col.bounds.extents.y + 0.1f)
        };

        foreach (RaycastHit2D[] hitArray in hits) //Check each array that contains all hits 
        {
            foreach (RaycastHit2D hit in hitArray) //Check each hit in each array 
            {
                if (hit.collider.gameObject == gameObject) continue; //If ray hits the current gameobject, ignore it 

                if (neighbours.Contains(this)) return; //If neighbour list contains the current block, exit function

                Block check = hit.collider.gameObject.GetComponent<Block>();

                if (check != null)
                {
                    if (check is ChainingBlock)
                    {
                        //Debug.Log($"Shooting ray from {gameObject.name}, block {hit.collider.gameObject.name} found");
                        neighbours.Add(check); //Add it to neighbour list 
                    }
                }
            }
        }
    }
    
    public override void Activate()
    {
        StartCoroutine(ActivateBlock(new HashSet<ChainingBlock>(), delay));
    }
    public IEnumerator ActivateBlock(HashSet<ChainingBlock> passedBlocks, WaitForSeconds timeBetweenActivations)
    {
        if (passedBlocks.Contains(this)) yield break; //If this object has been passed before, breaks out of recursion

        //Turning on stuff here
        yield return timeBetweenActivations;

        TurnOn();

        //Recursions handled here 
        passedBlocks.Add(this);

        foreach (ChainingBlock block in neighbours)
            block.StartCoroutine(block.ActivateBlock(passedBlocks, timeBetweenActivations));
    }
    public override void Deactivate()
    {
        Debug.Log("Can't deactivate through this function");
    }
    public IEnumerator Deactivate(HashSet<ChainingBlock> passedBlocks, WaitForSeconds timeBetweenDeactivations)
    {
        if (passedBlocks.Contains(this)) yield break; //If this object has been passed before, breaks out of recursion

        //Turning off stuff here 
        yield return timeBetweenDeactivations;

        TurnOff();

        //Recursions handled here 
        passedBlocks.Add(this);

        foreach (ChainingBlock block in neighbours)
            block.StartCoroutine(block.Deactivate(passedBlocks, timeBetweenDeactivations));
    }
    
    protected virtual void TurnOn()
    {
        spriteRenderer.color = activeColor;
        col.enabled = true;
    }
    protected virtual void TurnOff()
    {
        spriteRenderer.color = inactiveColor;
        col.enabled = false;
    }
}
