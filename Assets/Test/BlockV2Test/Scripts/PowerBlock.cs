using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBlock : Block
{
    private HashSet<Block> neighbours = new HashSet<Block>();
    public HashSet<Block> Neighbours
    { 
        get { return neighbours; }
        set { neighbours = value; }
    }
    private BoxCollider2D col; 
    
    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        //spriteRenderer = GetComponent<SpriteRenderer>();

        InitializeNeighbours(); //Initialize the neighbouring blocks for each block
    }
    protected void InitializeNeighbours() //Used to initialize the neighbouring blocks for each block
    {
        List<RaycastHit2D[]> hits = new List<RaycastHit2D[]>() //Contains everything that are beside each block
        {
            Physics2D.BoxCastAll(transform.position, transform.localScale * 0.9f, 0f, Vector2.left, col.bounds.extents.x + 0.1f),
            Physics2D.BoxCastAll(transform.position, transform.localScale * 0.9f, 0f, Vector2.right, col.bounds.extents.x + 0.1f),
            Physics2D.BoxCastAll(transform.position, transform.localScale * 0.9f, 0f, Vector2.up, col.bounds.extents.y + 0.1f),
            Physics2D.BoxCastAll(transform.position, transform.localScale * 0.9f, 0f, Vector2.down, col.bounds.extents.y + 0.1f)
        };

        foreach (RaycastHit2D[] hitArray in hits) //Check each array that contains all hits 
        {
            foreach (RaycastHit2D hit in hitArray) //Check each hit in each array 
            {
                if (hit.collider.gameObject == gameObject) continue; //If ray hits the current gameobject, ignore it 

                if (neighbours.Contains(this)) return; //If neighbour list contains the current block, exit function

                if (hit.collider.gameObject.GetComponent<Block>() != null)
                {
                    Debug.Log($"Shooting ray from {gameObject.name}, block {hit.collider.gameObject.name} found");
                    neighbours.Add(hit.collider.GetComponent<Block>()); //Add it to neighbour list 
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Activate();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            foreach (Block activatableBlocks in neighbours)
            {
                Deactivate();
            }
        }
    }
    public override void Activate()
    {
        foreach(Block block in neighbours)
        {
            block.Activate();
            //switch (block)
            //{
            //    case PlatformBlock platformBlock:
            //        platformBlock.Activate();
            //        break;

            //    case BridgingBlock bridgingBlock:
            //        bridgingBlock.Activate();
            //        break;

            //    case MissileBlock missileBlock:
            //        missileBlock.Activate();
            //        break;

            //    default:
            //        Debug.Log("No behaviour available, please create one");
            //        break;
            //}
        }
    }
    public override void Deactivate()
    {
        foreach (Block block in neighbours)
        {
            block.Deactivate();
            //switch (block)
            //{
            //    case PlatformBlock platformBlock:
            //        platformBlock.Deactivate();
            //        break;

            //    case BridgingBlock bridgingBlock:
            //        Debug.Log("Found a bridgingBlock, doing nothing");
            //        break;

            //    case MissileBlock missileBlock:
            //        missileBlock.Deactivate();
            //        break;

            //    default:
            //        Debug.Log("No behaviour available, please create one");
            //        break;
            //}
        }
    }
}
