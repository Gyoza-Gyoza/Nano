using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBlock : Block
{
    [SerializeField]
    protected Transform[] positions = new Transform[2];

    [SerializeField]
    protected float speed; private void OnCollisionEnter2D(Collision2D collision)
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
}
