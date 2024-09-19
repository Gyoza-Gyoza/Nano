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

    protected Dictionary<BridgingBlock, bool> connections;

    protected BoxCollider2D col;

    protected SpriteRenderer spriteRenderer;

    protected bool playerCollided = false,
        bridgeActivated = false;

    private BridgingBlock managerBlock;

    private WaitForSeconds delay;

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

        if (managerBlock == this) InitializeConnections(managerBlock, connections);

        delay = new WaitForSeconds(frequency);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (managerBlock != this) return;
            foreach (KeyValuePair<BridgingBlock, bool> kvp in connections)
            {
                Debug.Log(kvp.Key.name);
            }
        }
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
                        Debug.Log($"Powerblock found beside me, i'm {name}");
                        managerBlock = this;

                        connections = new Dictionary<BridgingBlock, bool>(); 
                    }
                    if(check is BridgingBlock)
                    {
                        //Debug.Log($"Shooting ray from {gameObject.name}, block {hit.collider.gameObject.name} found");
                        neighbours.Add(check); //Add it to neighbour list 
                    }
                }
            }
        }
    }
    public void InitializeConnections(BridgingBlock managerBlock, Dictionary<BridgingBlock, bool> connectionList)
    {        
        if (connectionList.ContainsKey(this)) return; //If dictionary contains the current block, exit function

        connectionList.Add(this, false);
        this.managerBlock = managerBlock;


        foreach (BridgingBlock block in neighbours)
            block.InitializeConnections(managerBlock, connectionList); //Continues to the next block 
    }
    protected void PlayerOnBlock(BridgingBlock block, bool state)
    {
        connections[block] = state;

        CheckContacts();
    }
    public override void Activate()
    {
        StartCoroutine(ActivateBlock(new HashSet<BridgingBlock>(), delay)); 
    }
    public IEnumerator ActivateBlock(HashSet<BridgingBlock> passedBlocks, WaitForSeconds timeBetweenActivations)
    {
        if (passedBlocks.Contains(this)) yield break; //If this object has been passed before, breaks out of recursion

        //Turning on stuff here
        yield return timeBetweenActivations;

        spriteRenderer.color = activeColor;
        col.enabled = true;

        //Recursions handled here 
        passedBlocks.Add(this);

        foreach (BridgingBlock block in neighbours)
            block.StartCoroutine(block.ActivateBlock(passedBlocks, timeBetweenActivations));
    }
    public override void Deactivate()
    {
        Debug.Log("Can't deactivate through this function");
    }
    public IEnumerator Deactivate(HashSet<BridgingBlock> passedBlocks, WaitForSeconds timeBetweenDeactivations)
    {
        if (passedBlocks.Contains(this)) yield break; //If this object has been passed before, breaks out of recursion

        //Turning off stuff here 
        yield return timeBetweenDeactivations;

        spriteRenderer.color = inactiveColor;
        col.enabled = false;

        //Recursions handled here 
        passedBlocks.Add(this);

        foreach (BridgingBlock block in neighbours)
            block.StartCoroutine(block.Deactivate(passedBlocks, timeBetweenDeactivations));
    }
    private void CheckContacts()
    {
        bool allFalse = true;

        foreach (KeyValuePair<BridgingBlock, bool> keyValuePair in connections) //Checks all the bools to see if there are any blocks that are being touched by the player 
        {
            if (keyValuePair.Value)
            {
                allFalse = false;
                break; //If its true, exit loop 
            }
        }

        if (allFalse)
        {
            StartCoroutine(Deactivate(new HashSet<BridgingBlock>(), delay));
        }
    }
}
