using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BridgingBlock : ChainingBlock
{
    protected BridgingBlock managerBlock;

    protected Dictionary<BridgingBlock, bool> connections;

    protected bool playerCollided = false,
        bridgeActivated = false;

    private void Start()
    {
        if (this is BridgingBlock) col.enabled = false;

        if (managerBlock == this) InitializeConnections(managerBlock, connections);

        delay = new WaitForSeconds(chainTime);
    }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") managerBlock.PlayerOnBlock(this, true);
    }
    protected void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") managerBlock.PlayerOnBlock(this, false);
    }
    protected override void InitializeNeighbours() //Used to initialize the neighbouring blocks for each block
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
                    if (check is ChainingBlock)
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
            StartCoroutine(Deactivate(new HashSet<ChainingBlock>(), delay));
        }
    }
    protected override void TurnOn()
    {
        spriteRenderer.color = activeColor;
        col.enabled = true;
    }
    protected override void TurnOff()
    {
        spriteRenderer.color = inactiveColor;
        col.enabled = false;
    }
}
