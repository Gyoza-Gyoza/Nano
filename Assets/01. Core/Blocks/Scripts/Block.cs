using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] protected Color activeColor, inactiveColor;
    [SerializeField] protected Sprite activeSprite, inactiveSprite;

    [SerializeField] protected Material activeMaterial, inactiveMaterial;

    //Keeps a list of neighbouring blocks 
    protected HashSet<Block> neighbours = new HashSet<Block>();

    protected BoxCollider2D col;

    protected SpriteRenderer spriteRenderer;

    [SerializeField]
    private float chargeTime = 0.03f;
    private WaitForSeconds delay;

    [SerializeField]
    protected bool isCharged = false;

    public static bool drained = false;

    protected Dictionary<Block, bool> connections = new Dictionary<Block, bool>();
    public Dictionary<Block, bool> Connections
    { get { return connections; } }

    protected PowerBlock managerBlock;

    public virtual bool IsCharged
    {
        get { return isCharged; }
        set
        {
            isCharged = value;

            if (IsCharged) Activate();
            else Deactivate();
        }
    }

    protected virtual void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        InitializeNeighbours(); //Initialize the neighbouring blocks for each block

        delay = new WaitForSeconds(chargeTime);
    }
    protected virtual void InitializeNeighbours() //Used to initialize the neighbouring blocks for each block
    {
        List<RaycastHit2D[]> hits = new List<RaycastHit2D[]>() //Contains everything that are beside each block
        {
            Physics2D.BoxCastAll(transform.position, new Vector2(0.1f, transform.localScale.y * 0.9f), 0f, Vector2.right, col.bounds.extents.x + 0.1f),
            Physics2D.BoxCastAll(transform.position, new Vector2(0.1f, transform.localScale.y * 0.9f), 0f, Vector2.left, col.bounds.extents.x + 0.1f),
            Physics2D.BoxCastAll(transform.position, new Vector2(transform.localScale.x * 0.9f, 0.1f), 0f, Vector2.up, col.bounds.extents.y + 0.1f),
            Physics2D.BoxCastAll(transform.position, new Vector2(transform.localScale.x * 0.9f, 0.1f), 0f, Vector2.down, col.bounds.extents.y + 0.1f)
        };

        foreach (RaycastHit2D[] hitArray in hits) //Check each array that contains all hits 
        {
            foreach (RaycastHit2D hit in hitArray) //Check each hit in each array 
            {
                if (hit.collider.gameObject == gameObject) continue; //If ray hits the current gameobject, ignore it 

                if (neighbours.Contains(this)) return; //If neighbour list contains the current block, exit function

                Block check = hit.collider.gameObject.GetComponent<Block>();

                if (check != null) neighbours.Add(check); //Add it to neighbour list 
            }
        }
    }
    public void InitializeConnections(PowerBlock managerBlock, Dictionary<Block, bool> connectionList)
    {
        if (connectionList.ContainsKey(this)) return; //If dictionary contains the current block, exit function

        connectionList.Add(this, false);
        this.managerBlock = managerBlock;


        foreach (Block block in neighbours)
        {
            if (block is BridgingBlock)
            {
                BridgingBlock bridgingBlock = block as BridgingBlock;
                bridgingBlock.InitializeConnections(managerBlock, connectionList); //Continues to the next block 
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") StartCoroutine(Charge(new HashSet<Block>()));
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") StartCoroutine(Discharge(new HashSet<Block>()));
    }
    public IEnumerator Charge(HashSet<Block> passedBlocks)
    {
        if (passedBlocks.Contains(this)) yield break;
        if (IsCharged) yield break;
        passedBlocks.Add(this);

        yield return delay;

        IsCharged = true;

        foreach (Block block in neighbours)
            block.StartCoroutine(block.Charge(passedBlocks));
    }

    public IEnumerator Discharge(HashSet<Block> passedBlocks)
    {
        if (passedBlocks.Contains(this)) yield break;
        if (!IsCharged) yield break;
        passedBlocks.Add(this);

        IsCharged = false;

        yield return delay;

        foreach (Block block in neighbours)
            block.StartCoroutine(block.Discharge(passedBlocks));
    }

    public virtual void Activate()
    {
        StartCoroutine(ActivateBlock());
    }
    public virtual IEnumerator ActivateBlock()
    {
        yield return null;
    }
    public virtual void Deactivate()
    {
        StartCoroutine(DeactivateBlock());
    }
    public virtual IEnumerator DeactivateBlock()
    {
        yield return null;
    }
    public void PlayerOnBlock(Block block, bool state)
    {
        connections[block] = state;

        CheckContacts();
    }
    private void CheckContacts()
    {
        foreach (KeyValuePair<Block, bool> keyValuePair in connections) //Checks all the bools to see if there are any blocks that are being touched by the player 
        {
            if (keyValuePair.Value) //If a true is found, exit function
            {
                return;
            }
        }
        //If loop completes, means there are no trues and discharges the blocks
        foreach (Block block in neighbours)
        {
            if (block is BridgingBlock) block.StartCoroutine(Discharge(new HashSet<Block>()));
        }
    }
}