using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStartScriptLegacy : MonoBehaviour, IBlockLegacy
{
    [SerializeField]
    protected BlockStateLegacy currentState;
    public BlockStateLegacy CurrentState
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
    protected float timeToChange; 

    [SerializeField]
    protected Color activeColor,
        inactiveColor, 
        startColor;

    private bool playerInContact = false;

    protected SpriteRenderer spriteRenderer;
    protected BoxCollider2D col;

    protected List<BlockScriptLegacy> neighbours = new List<BlockScriptLegacy>();

    [SerializeField]
    private List<BlockScriptLegacy> allNeighbours = new List<BlockScriptLegacy>();

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();

        InitializeNeighbours();
        UpdateBlockState();

        Debug.Log($"{gameObject.name}'s BlockStartScript called");
    }
    protected void InitializeNeighbours() //Used to initialize the neighbouring blocks for each block
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
            foreach (RaycastHit2D hit in hitArray) //Check each hit in each array 
            {
                if (hit.collider.gameObject == gameObject) continue;
                IBlockLegacy iBlock = hit.collider.gameObject.GetComponent<IBlockLegacy>(); //Check for iBlock interface 
                if (iBlock != null)
                {
                    Debug.Log($"Shooting ray from {gameObject.name}, iblock {hit.collider.gameObject.name} found");
                    neighbours.Add(hit.collider.GetComponent<BlockScriptLegacy>()); //Add it to neighbour list 
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState != BlockStateLegacy.Start) return;

        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Starting current");
            foreach (BlockScriptLegacy hit in neighbours)
            {
                Debug.Log("Checking neighbours");
                if (hit != null) hit.StartCoroutine(hit.ChangeBlockState(BlockStateLegacy.Active, new WaitForSeconds(timeToChange)));
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (currentState != BlockStateLegacy.Start) return;

        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Starting current");
            foreach (BlockScriptLegacy hit in neighbours)
            {
                Debug.Log("Checking neighbours");
                if (hit != null) hit.StartCoroutine(hit.ChangeBlockState(BlockStateLegacy.Inactive, new WaitForSeconds(timeToChange)));
            }
        }
    }

    public void UpdateBlockState()
    {
        switch (currentState)
        {
            case BlockStateLegacy.Active:
                spriteRenderer.color = activeColor;
                col.isTrigger = false;
                //gameObject.layer = 6; 
                break;

            case BlockStateLegacy.Inactive:
                spriteRenderer.color = inactiveColor;
                col.isTrigger = true;
                //gameObject.layer = 0; 
                break;

            case BlockStateLegacy.Start:
                spriteRenderer.color = startColor;
                //col.enabled = false;
                //gameObject.layer = 0; 
                break;
        }
    }


    public IEnumerator ChangeBlockState(BlockStateLegacy newState, WaitForSeconds timeToWait)
    {
        Debug.Log($"Current state is {CurrentState} \n State to change is {newState}");
        if (currentState == newState || currentState == BlockStateLegacy.Start) yield break;

        //Activate neighbours
        foreach (BlockScriptLegacy hit in neighbours)
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
}
