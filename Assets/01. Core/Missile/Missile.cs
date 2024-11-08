using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public sealed class Missile : MonoBehaviour
{
    [SerializeField]
    private float missileSpeed;
    public float MissileSpeed
    { get { return missileSpeed; } set { missileSpeed = value; } }

    private float lifeTime = 7f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerBehaviour.player.DamageBattery();
            Destroy(gameObject);
        }

        else if (collision.CompareTag("Charged Ground"))
        {
            Destroy(gameObject);
        }

        //Block blockHit = collision.GetComponentInParent<PlatformBlock>();

        //if (blockHit != null && blockHit.IsCharged)
        //{
        //    Destroy(gameObject);
        //}
    }
    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x - missileSpeed, transform.position.y, transform.position.z); 
    }
}
