using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockScript : MonoBehaviour, IBlock
{
    [SerializeField]
    private BlockState currentState;
    public BlockState CurrentState
    {
        get
        { return currentState; }
        set
        {
            currentState = value;
            UpdateBlockState();
        }
    }

    [SerializeField]
    private float timeToChange; 

    [SerializeField]
    private Color activeColor,
        inactiveColor, 
        startColor; 

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D col; 

    private List<BlockScript> neighbours = new List<BlockScript>();

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();

        InitializeNeighbours();
        UpdateBlockState();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState != BlockState.Start) return;

        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Starting current");
            foreach (BlockScript hit in neighbours)
            {
                Debug.Log("Checking neighbours");
                if(hit != null) hit.StartCoroutine(hit.ChangeBlockState(BlockState.Active, new WaitForSeconds(timeToChange)));
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (currentState != BlockState.Start) return;

        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Starting current");
            foreach (BlockScript hit in neighbours)
            {
                Debug.Log("Checking neighbours");
                if (hit != null) hit.StartCoroutine(hit.ChangeBlockState(BlockState.Inactive, new WaitForSeconds(timeToChange)));
            }
        }
    }
    private void InitializeNeighbours() //Used to initialize the neighbouring blocks for each block
    {
        List<RaycastHit2D[]> hits = new List<RaycastHit2D[]>() //Contains everything that are beside each block
        {
            Physics2D.RaycastAll(transform.position, Vector2.left, col.bounds.extents.x + 0.1f),
            Physics2D.RaycastAll(transform.position, Vector2.right, col.bounds.extents.x + 0.1f),
            Physics2D.RaycastAll(transform.position, Vector2.up, col.bounds.extents.y + 0.1f),
            Physics2D.RaycastAll(transform.position, Vector2.down, col.bounds.extents.y + 0.1f)
        };

        foreach (RaycastHit2D[] hitArray in hits) //Check each array that contains all hits 
        {
            foreach(RaycastHit2D hit in hitArray) //Check each hit in each array 
            {
                if (hit.collider.gameObject == gameObject) continue;
                IBlock iBlock = hit.collider.gameObject.GetComponent<IBlock>(); //Check for iBlock interface 
                if (iBlock != null) 
                {
                    Debug.Log($"Shooting ray from {gameObject.name}, iblock {hit.collider.gameObject.name} found");
                    neighbours.Add(hit.collider.GetComponent<BlockScript>()); //Add it to neighbour list 
                }
            }
        }
    }
    public IEnumerator ChangeBlockState(BlockState newState, WaitForSeconds timeToWait)
    {
        Debug.Log($"Current state is {CurrentState.ToString()} \n State to change is {newState}");
        if (currentState == newState || currentState == BlockState.Start) yield break;

        //Activate neighbours
        foreach (BlockScript hit in neighbours)
        {
            Debug.Log("Entered loop");
            if (hit != null) //Check if something is hit
            {
                CurrentState = newState;
                Debug.Log($"{gameObject.name} activated");

                yield return timeToWait;
                hit.StartCoroutine(hit.ChangeBlockState(newState, timeToWait));
            }
        }
    }
    public void UpdateBlockState()
    {
        switch (currentState)
        {
            case BlockState.Active:
                spriteRenderer.color = activeColor;
                //col.enabled = true;
                //gameObject.layer = 6; 
                break;

            case BlockState.Inactive:
                spriteRenderer.color = inactiveColor;
                //col.enabled = false;
                //gameObject.layer = 0; 
                break;
            
            case BlockState.Start:
                spriteRenderer.color = startColor;
                //col.enabled = false;
                //gameObject.layer = 0; 
                break;
        }
    }
}