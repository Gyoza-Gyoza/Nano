using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    [SerializeField]
    protected Color activeColor,
        inactiveColor;

    [SerializeField]
    protected BlockState currentState; 

    public BlockState CurrentState
    { 
        get { return currentState; } 
        set 
        { 
            currentState = value; 
        } 
    }

    //Keeps a list of neighbouring blocks 
    [SerializeField] //Remove after testing
    protected HashSet<BlockBehaviour> neighbours = new HashSet<BlockBehaviour>();

    protected BoxCollider2D col;

    protected SpriteRenderer spriteRenderer; 

    protected bool playerCollided = false,
        bridgeActivated = false;

    private PowerBlockBehaviour powerBlock;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        InitializeNeighbours(); //Initialize the neighbouring blocks for each block
    }

    private void Start()
    {
        if (this is not PowerBlockBehaviour) col.enabled = false;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player") powerBlock.PlayerOnBlock(this, true);
    }
    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") powerBlock.PlayerOnBlock(this, false);
    }

    protected void InitializeNeighbours() //Used to initialize the neighbouring blocks for each block
    {
        List<RaycastHit2D[]> hits = new List<RaycastHit2D[]>() //Contains everything that are beside each block
        {
            Physics2D.BoxCastAll(transform.position, transform.localScale, 0f, Vector2.left, col.bounds.extents.x + 0.1f),
            Physics2D.BoxCastAll(transform.position, transform.localScale, 0f, Vector2.right, col.bounds.extents.x + 0.1f),
            Physics2D.BoxCastAll(transform.position, transform.localScale, 0f, Vector2.up, col.bounds.extents.y + 0.1f),
            Physics2D.BoxCastAll(transform.position, transform.localScale, 0f, Vector2.down, col.bounds.extents.y + 0.1f)
        };

        foreach (RaycastHit2D[] hitArray in hits) //Check each array that contains all hits 
        {
            foreach (RaycastHit2D hit in hitArray) //Check each hit in each array 
            {
                if (hit.collider.gameObject == gameObject) continue; //If ray hits the current gameobject, ignore it 

                if (neighbours.Contains(this)) return; //If neighbour list contains the current block, exit function

                if (hit.collider.gameObject.GetComponent<BlockBehaviour>() != null)
                {
                    Debug.Log($"Shooting ray from {gameObject.name}, iblock {hit.collider.gameObject.name} found");
                    neighbours.Add(hit.collider.GetComponent<BlockBehaviour>()); //Add it to neighbour list 
                }
            }
        }
    }
    public void GetConnections(PowerBlockBehaviour powerBlock, Dictionary<BlockBehaviour, bool> connectionList)
    {
        if (connectionList.ContainsKey(this)) return; //If dictionary contains the current block, exit function

        this.powerBlock = powerBlock;

        connectionList.Add(this, false);

        foreach (BlockBehaviour block in neighbours) 
            block.GetConnections(powerBlock, connectionList); //Continues to the next block 
    }

    public virtual IEnumerator Activate(HashSet<BlockBehaviour> passedBlocks, WaitForSeconds timeBetweenActivations)
    {
        if(passedBlocks.Contains(this)) yield break; //If this object has been passed before, breaks out of recursion

        Debug.Log($"{gameObject.name} activated");

        //Turning on stuff here
        yield return timeBetweenActivations;
        if (currentState != BlockState.Power)
        {
            spriteRenderer.color = activeColor;
            col.enabled = true;
        }

        //Recursions handled here 
        passedBlocks.Add(this);

        foreach (BlockBehaviour block in neighbours)
            block.StartCoroutine(block.Activate(passedBlocks, timeBetweenActivations)); 
    }

    public virtual IEnumerator Deactivate(HashSet<BlockBehaviour> passedBlocks, WaitForSeconds timeBetweenDeactivations)
    {
        if (passedBlocks.Contains(this)) yield break; //If this object has been passed before, breaks out of recursion

        Debug.Log($"{gameObject.name} deactivated");

        //Turning off stuff here 
        yield return timeBetweenDeactivations;
        if (currentState != BlockState.Power)
        {
            spriteRenderer.color = inactiveColor;
            col.enabled = false;
        }

        //Recursions handled here 
        passedBlocks.Add(this);

        foreach (BlockBehaviour block in neighbours)
            block.StartCoroutine(block.Deactivate(passedBlocks, timeBetweenDeactivations));
    }
}
public enum BlockState
{
    Inactive,
    Active,
    Power
}