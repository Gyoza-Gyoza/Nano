using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayer : MonoBehaviour
{
    [SerializeField] private float movementSpeed, jumpHeight;
    [SerializeField] private int maxJumps;   // Start is called before the first frame update
    [SerializeField] private LayerMask groundMask;
    private int jumpCounter = 0;
    private BoxCollider2D col;
    private Rigidbody2D rb;
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(-movementSpeed, rb.velocity.y);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpCounter >= maxJumps)
            {
                return;
            }

            rb.AddForce(new Vector2(0f, jumpHeight));
            jumpCounter++;
        }

        if (Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - col.bounds.extents.y * 0.5f), transform.localScale * 0.5f, 0f, Vector2.down, 0.01f, groundMask))
        {
            jumpCounter = 0;
        }

    }
}
