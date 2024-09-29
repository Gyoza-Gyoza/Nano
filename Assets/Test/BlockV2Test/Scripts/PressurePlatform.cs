using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlatform : PlatformBlock
{
    private bool ascending = false;

    [Range(0f, 1f)]
    public float currentPos; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            IsCharged = true;
            collision.gameObject.transform.SetParent(gameObject.transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            IsCharged = false;
            collision.gameObject.transform.SetParent(null);
        }
    }
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        //If its ascending, change the speed to positive and if its descending, change the speed to negative
        float timeChange = ascending ? Time.deltaTime * speed : -Time.deltaTime * speed;

        //Changes currentPost based on whether its ascending or descending
        //Lerp is based on this
        currentPos = Mathf.Clamp(currentPos + timeChange, 0f, 1f);

        //if (Vector3.Distance(transform.position, positions[1].position) <= 0.05f && ascending)
        //    transform.position = positions[1].position;

        //else if (Vector3.Distance(transform.position, positions[0].position) <= 0.05f && !ascending)
        //    transform.position = positions[0].position;

        //else
            transform.position = Vector3.Lerp(positions[0].position, positions[1].position, currentPos);
    }
    public override void Activate()
    {
        ascending = true;
    }
    public override void Deactivate()
    {
        ascending = false;
    }
}
