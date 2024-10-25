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
        platformRenderer.sprite = activePlatformSprite;
        pillarRenderer.sprite = activePillarSprite;

        platformRenderer.material = activePlatformMat;
        pillarRenderer.material = activePillarMat;
    }
    public override void Deactivate()
    {
        platformRenderer.sprite = inactivePlatformSprite;
        pillarRenderer.sprite = inactivePillarSprite;

        platformRenderer.material = inactivePlatformMat;
        pillarRenderer.material = inactivePillarMat;
    }
    protected override void Update()
    {
        base.Update();
        if (IsCharged) Move();
    }
    private void Move()
    {
        float timeChange = ascending ? Time.deltaTime * speed : -Time.deltaTime * speed;
        currentPos = Mathf.Clamp(currentPos + timeChange, 0f, 1f);

        if (Vector2.Distance(transform.position, positions[1].position) <= 0.05f && ascending)
        {
            transform.position = positions[1].position;
            ascending = false;
        }
        else if (Vector2.Distance(transform.position, positions[0].position) <= 0.05f && !ascending)
        {
            transform.position = positions[0].position;
            ascending = true;
        }

        else
            transform.position = Vector2.Lerp(positions[0].position, positions[1].position, currentPos);
    }
}

//Get the difference in x and y values of the start and end position 
//Lerp between these two values and 