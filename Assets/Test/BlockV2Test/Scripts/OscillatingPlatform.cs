using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillatingPlatform : PlatformBlock
{
    private bool ascending = false, 
        playerOnBlock = false;

    [Range(0f, 1f)]
    public float currentPos;

    public override void Activate()
    {
        playerOnBlock = true;
    }
    public override void Deactivate()
    {
        playerOnBlock = false;
    }
    private void Update()
    {
        if (playerOnBlock) Move();
    }
    private void Move()
    {
        float timeChange = ascending ? Time.deltaTime * speed : -Time.deltaTime * speed;
        currentPos = Mathf.Clamp(currentPos + timeChange, 0f, 1f);

        if (Vector3.Distance(transform.position, positions[1].position) <= 0.05f && ascending)
        {
            transform.position = positions[1].position;
            ascending = false;
        }
        else if (Vector3.Distance(transform.position, positions[0].position) <= 0.05f && !ascending)
        {
            transform.position = positions[0].position;
            ascending = true;
        }

        else
            transform.position = Vector3.Lerp(positions[0].position, positions[1].position, currentPos);
    }
}