using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlatformBlock : Block
{
    [Header("Sprites for Platform and Pillar")]
    [SerializeField] protected Sprite activePlatformSprite;
    [SerializeField] protected Sprite inactivePlatformSprite;
    [SerializeField] protected Sprite activePillarSprite;
    [SerializeField] protected Sprite inactivePillarSprite;
    [SerializeField] protected Material activePlatformMat;
    [SerializeField] protected Material inactivePlatformMat;
    [SerializeField] protected Material activePillarMat;
    [SerializeField] protected Material inactivePillarMat;

    protected SpriteRenderer platformRenderer;
    protected SpriteRenderer pillarRenderer;

    [SerializeField]
    protected Transform[] positions = new Transform[2];

    [SerializeField]
    protected float speed;
    void Start()
    {
        // Get the SpriteRenderer components for the children
        platformRenderer = transform.Find("Platform").GetComponent<SpriteRenderer>();
        pillarRenderer = transform.Find("Pillar").GetComponent<SpriteRenderer>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            IsCharged = true;
            PlayerBehaviour.player.transform.SetParent(transform);
            PlayerBehaviour.player.rb.interpolation = RigidbodyInterpolation2D.None; 
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            IsCharged = false;
            PlayerBehaviour.player.transform.SetParent(null);
            PlayerBehaviour.player.rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }
}
