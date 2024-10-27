using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlatformBlock : Block
{
    [Header("Sprites for Platform and Pillar")]
    [SerializeField] protected Sprite activePlatformSprite;
    [SerializeField] protected Sprite inactivePlatformSprite;
    [SerializeField] protected Sprite activePillarSprite;
    [SerializeField] protected Sprite inactivePillarSprite;
    [SerializeField] protected Material activePlatformMat;
    [SerializeField] protected Material inactivePlatformMat;
    [SerializeField] protected Material activePillarMat;
    [SerializeField] protected Material inactivePillarMat;

    protected SpriteRenderer platformRenderer;
    protected SpriteRenderer pillarRenderer;

    [SerializeField]
    protected Transform[] positions = new Transform[2];

    [SerializeField]
    protected float speed;
    void Start()
    {
        // Get the SpriteRenderer components for the children
        platformRenderer = transform.Find("Platform").GetComponent<SpriteRenderer>();
        pillarRenderer = transform.Find("Pillar").GetComponent<SpriteRenderer>();
    }
    protected virtual void Update()
    {
        //if (IsCharged) ChargeSurroundings();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            IsCharged = true;
            PlayerBehaviour.player.transform.SetParent(transform);
            PlayerBehaviour.player.rb.interpolation = RigidbodyInterpolation2D.None; 
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            IsCharged = false;
            PlayerBehaviour.player.transform.SetParent(null);
            PlayerBehaviour.player.rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }
    private void ChargeSurroundings()
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
                Block check = hit.collider.gameObject.GetComponent<Block>();

                if (check != null) check.StartCoroutine(check.Charge(new HashSet<Block>()));
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Block block = collision.GetComponent<Block>();

        if (block == null) return;
        Debug.Log(collision.name);
        if (block == this) return;
        if (!isCharged) return;

        block.Charge(new HashSet<Block>());
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Block block = collision.GetComponent<Block>();

        if (block == null) return;
        if (block == this) return;
        if (!isCharged) return;

        block.Discharge(new HashSet<Block>());
    }
}
