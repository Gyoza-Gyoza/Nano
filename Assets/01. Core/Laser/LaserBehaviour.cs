using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    private BoxCollider2D col; 

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerBehaviour.player.PlayerDeath();
        }
    }
}
