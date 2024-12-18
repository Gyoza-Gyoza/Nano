using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public sealed class PressurePlatform : PlatformBlock
{
    private bool ascending = false;

    [Range(0f, 1f)]
    public float currentPos; 

    protected override void Update()
    {
        base.Update();
        Move();
    }
    private void Move()
    {
        //If its ascending, change the speed to positive and if its descending, change the speed to negative
        float timeChange = ascending ? Time.deltaTime * speed : -Time.deltaTime * speed;

        //Changes currentPos based on whether its ascending or descending
        //Lerp is based on this
        currentPos = Mathf.Clamp(currentPos + timeChange, 0f, 1f);

        transform.position = Vector2.Lerp(positions[0].position, positions[1].position, currentPos);
    }
    public override void Activate()
    {
        platformRenderer.sprite = activePlatformSprite;
        pillarRenderer.sprite = activePillarSprite;

        platformRenderer.material = activePlatformMat;
        pillarRenderer.material = activePillarMat;

        ascending = true;
    }
    public override void Deactivate()
    {
        platformRenderer.sprite = inactivePlatformSprite;
        pillarRenderer.sprite = inactivePillarSprite;

        platformRenderer.material = inactivePlatformMat;
        pillarRenderer.material = inactivePillarMat;

        ascending = false;
    }
}
