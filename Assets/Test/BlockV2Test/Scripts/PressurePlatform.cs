using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlatform : PlatformBlock
{
    private bool ascending = false;

    [Header("Sprites for Platform and Pillar")]
    [SerializeField] private Sprite activePlatformSprite;
    [SerializeField] private Sprite inactivePlatformSprite;
    [SerializeField] private Sprite activePillarSprite;
    [SerializeField] private Sprite inactivePillarSprite;
    [SerializeField] private Material activePlatformMat;
    [SerializeField] private Material inactivePlatformMat;
    [SerializeField] private Material activePillarMat;
    [SerializeField] private Material inactivePillarMat;

    private SpriteRenderer platformRenderer;
    private SpriteRenderer pillarRenderer;

    [Range(0f, 1f)]
    public float currentPos; 

    void Start()
    {
        // Get the SpriteRenderer components for the children
        platformRenderer = transform.Find("Platform").GetComponent<SpriteRenderer>();
        pillarRenderer = transform.Find("Pillar").GetComponent<SpriteRenderer>();
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
