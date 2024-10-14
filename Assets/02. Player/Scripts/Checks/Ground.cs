using UnityEngine;

public class Ground : MonoBehaviour
{
    private bool onGround;
    private bool wasGrounded = true;
    private float friction;
    private float groundBufferTime = 0.1f;
    private float lastGroundedTime = 0f;
    private bool isJumping = false;
    private Collision2D lastCollision;

    private void FixedUpdate()
    {
        if (!onGround && Time.time - lastGroundedTime < groundBufferTime && !isJumping)
        {
            onGround = true;
        }

        if (onGround && !wasGrounded && !isJumping)
        {
            PlayLandingSound(lastCollision);
        }

        // Update wasGrounded flag for next frame
        wasGrounded = onGround;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);

        if (onGround)
        {
            isJumping = false;
            lastCollision = collision;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        lastGroundedTime = Time.time;
        onGround = false;
        friction = 0;
    }

    private void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;
            if (normal.y >= 0.9f)
            {
                onGround = true;
            }
        }

        //Ask Jiale what is this

        // for(int i = 0; i < collision.contactCount; i++)
        // {
        //     Vector2 normal = collision.GetContact(i).normal;
        //     onGround |= normal.y >= 0.9f;
        // }
    }

    private void RetrieveFriction(Collision2D collision)
    {
        PhysicsMaterial2D material = collision.rigidbody.sharedMaterial;
        friction = 0;

        if (material != null)
        {
            friction = material.friction;
        }
    }

    private void PlayLandingSound(Collision2D collision)
    {
        if (collision != null)
        {
            string groundTag = collision.gameObject.tag;

            switch (groundTag)
            {
                case "Ground":
                    SoundManager.PlaySound(SoundType.LAND_GROUND);
                    break;

                case "Charged Ground":
                    SoundManager.PlaySound(SoundType.LAND_CHARGE);
                    break;

                default:
                    SoundManager.PlaySound(SoundType.LAND_GROUND); // Default sound
                    break;
            }
        }
    }

    public bool GetOnGround()
    {
        return onGround;
    }

    public float GetFriction()
    {
        return friction;
    }

    public void TriggerJump()
    {
        isJumping = true;
    }
}
