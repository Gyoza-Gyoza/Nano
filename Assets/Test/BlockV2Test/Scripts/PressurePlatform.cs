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
            isDraining = true;
            IsCharged = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isDraining = false;
            IsCharged = false;
        }
    }
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        float timeChange = ascending ? Time.deltaTime * speed : -Time.deltaTime * speed;
        currentPos = Mathf.Clamp(currentPos + timeChange, 0f, 1f);

        if (Vector3.Distance(transform.position, positions[1].position) <= 0.05f && ascending)
            transform.position = positions[1].position;

        else if (Vector3.Distance(transform.position, positions[0].position) <= 0.05f && !ascending)
            transform.position = positions[0].position;

        else
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
