using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BridgingBlock : Block
{
    [SerializeField]
    protected Color activeColor,
        inactiveColor;

    //Keeps a list of neighbouring blocks 
    protected HashSet<Block> neighbours = new HashSet<Block>();

    protected Dictionary<Block, bool> connections;

    protected BoxCollider2D col;

    protected SpriteRenderer spriteRenderer;

    protected bool playerCollided = false,
        bridgeActivated = false;

    private BridgingBlock managerBlock;

    [SerializeField]
    private float frequency; 

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        InitializeNeighbours(); //Initialize the neighbouring blocks for each block
    }

    private void Start()
    {
        if (this is BridgingBlock) col.enabled = false;

        if(managerBlock == this) InitializeConnections(managerBlock, connections);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") managerBlock.PlayerOnBlock(this, true);
    }
    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") managerBlock.PlayerOnBlock(this, false);
    }

    protected void InitializeNeighbours() //Used to initialize the neighbouring blocks for each block
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
                    if (check is PowerBlock) //Assigns this block as the managerBlock if it is the first block in the chain
                    {
                        managerBlock = this;

                        connections = new Dictionary<Block, bool>(); 
                    }
                    if(check is BridgingBlock)
                    {
                        Debug.Log($"Shooting ray from {gameObject.name}, block {hit.collider.gameObject.name} found");
                        neighbours.Add(check); //Add it to neighbour list 
                    }
                }
            }
        }
    }
    public void InitializeConnections(BridgingBlock managerBlock, Dictionary<Block, bool> connectionList)
    {
        if (connectionList.ContainsKey(this)) return; //If dictionary contains the current block, exit function

        connectionList.Add(this, false);

        foreach (BridgingBlock block in neighbours)
            block.InitializeConnections(managerBlock, connectionList); //Continues to the next block 
    }
    protected void PlayerOnBlock(BridgingBlock block, bool state)
    {
        connections[block] = state;
    }
    public override void Activate()
    {
        StartCoroutine(ActivateBlock(new HashSet<BridgingBlock>(), new WaitForSeconds(frequency))); 
    }
    public IEnumerator ActivateBlock(HashSet<BridgingBlock> passedBlocks, WaitForSeconds timeBetweenActivations)
    {
        if (passedBlocks.Contains(this)) yield break; //If this object has been passed before, breaks out of recursion

        Debug.Log($"{gameObject.name} activated");

        //Turning on stuff here
        yield return timeBetweenActivations;

        spriteRenderer.color = activeColor;
        col.enabled = true;

        //Recursions handled here 
        passedBlocks.Add(this);

        foreach (BridgingBlock block in neighbours)
            block.StartCoroutine(block.ActivateBlock(passedBlocks, timeBetweenActivations));
    }

    public IEnumerator Deactivate(HashSet<BridgingBlock> passedBlocks, WaitForSeconds timeBetweenDeactivations)
    {
        if (passedBlocks.Contains(this)) yield break; //If this object has been passed before, breaks out of recursion

        Debug.Log($"{gameObject.name} deactivated");

        //Turning off stuff here 
        yield return timeBetweenDeactivations;

        spriteRenderer.color = inactiveColor;
        col.enabled = false;

        //Recursions handled here 
        passedBlocks.Add(this);

        foreach (BridgingBlock block in neighbours)
            block.StartCoroutine(block.Deactivate(passedBlocks, timeBetweenDeactivations));
    }
}
