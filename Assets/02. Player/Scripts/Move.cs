using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Move : MonoBehaviour
{

    [SerializeField] private InputController input = null;
    [SerializeField, Range(0f, 100f)] public float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;

    private Animator animator;

    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private Rigidbody2D body;
    private Ground ground;

    private float maxSpeedChange;
    private float acceleration;
    private bool onGround;
    private bool facingRight = true;

    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        direction.x = input.RetrieveMoveInput();
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - ground.GetFriction(), 0f);

        animator.SetFloat("Speed", Mathf.Abs(velocity.x));

        if (direction.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && facingRight)
        {
            Flip();
        }

        animator.SetBool("isJumping", !onGround);

        // Check for landing
        if (onGround && animator.GetBool("isJumping"))
        {
            animator.SetTrigger("isLanding");
        }        
    }

    private void FixedUpdate()
    {
        onGround = ground.GetOnGround();
        velocity = body.velocity;

        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        body.velocity = velocity;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
