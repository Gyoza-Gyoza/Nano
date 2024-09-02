using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed,
        jumpHeight;

    [SerializeField]
    private int maxJumps; 

    private int jumpCounter = 0; 

    private Rigidbody2D rb;

    public static PlayerBehaviour player; 
    private void Start()
    {
        if(player == null)
        {
            player = this;
        }
        else if (player != this)
        {
            Destroy(this);
        }

        rb = GetComponent<Rigidbody2D>(); 
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddForce(new Vector2(-movementSpeed, 0f)); 
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForce(new Vector2(movementSpeed, 0f));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(jumpCounter >= maxJumps)
            {
                return;
            }

            rb.AddForce(new Vector2(0f, jumpHeight));
            jumpCounter++; 
        }
        //if(Physics2D.BoxCast(transform.position, transform.localScale * 0.5f, Vector2.down))
        //{

        //}
    }
}
