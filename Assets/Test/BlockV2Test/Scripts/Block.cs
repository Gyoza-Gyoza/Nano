using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Block : MonoBehaviour
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

    private bool isCharged = false;

    public bool IsCharged
    {
        get { return isCharged; }
        set
        {
            isCharged = value;

            if (IsCharged) Activate();
            else Deactivate();
        }
    }

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        InitializeNeighbours(); //Initialize the neighbouring blocks for each block
        Debug.Log($"My name is {name} I have {neighbours.Count} neighbours");
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

    public void Charge()
    {
        if (IsCharged) return;

        IsCharged = true;

        Debug.Log("Im charged");
        IsCharged = true;
        foreach (Block block in neighbours)
            block.Charge();
    }

    public void Discharge()
    {
        if (!IsCharged) return;

        IsCharged = false; 

        Debug.Log("Im discharged");
        foreach (Block block in neighbours)
            block.Discharge();
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
}