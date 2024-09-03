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
            UpdateBlockState();
        }
    }

    [SerializeField]
    private Color activeColor,
        inactiveColor; 

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
        if(collision.gameObject.tag == "Player")
        {
            StartCoroutine(ChangeBlockState(BlockState.Active));
        }
    }
    private void InitializeNeighbours()
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>()
        {
            Physics2D.Raycast(transform.position, Vector2.left, col.bounds.extents.x + 0.1f, 7),
            Physics2D.Raycast(transform.position, Vector2.right, col.bounds.extents.x + 0.1f, 7),
            Physics2D.Raycast(transform.position, Vector2.up, col.bounds.extents.y + 0.1f, 7),
            Physics2D.Raycast(transform.position, Vector2.down, col.bounds.extents.y + 0.1f, 7)
        };

        foreach (RaycastHit2D hit in hits)
        {
            IBlock iBlock = hit.collider.gameObject.GetComponent<IBlock>();
            if (iBlock != null)
            {
                Debug.Log("iblock found");
                //neighbours.Add(hit.collider.GetComponent<BlockScript>());
            }
        }
    }
    public IEnumerator ChangeBlockState(BlockState newState)
    {
        //Loop through the hits
        foreach(BlockScript hit in neighbours)
        {
            if (hit == null) //Check if something is hit
            {
                yield break;
            }

            if (hit.CurrentState == BlockState.Inactive) //Check if the state is inactive
            {
                CurrentState = newState;
                Debug.Log($"{gameObject.name} hit");

                yield return new WaitForSeconds(0.2f);
                hit.StartCoroutine(ChangeBlockState(newState));
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
                gameObject.layer = 6; 
                break;

            case BlockState.Inactive:
                spriteRenderer.color = inactiveColor;
                //col.enabled = false;
                gameObject.layer = 0; 
                break;
        }
    }
}