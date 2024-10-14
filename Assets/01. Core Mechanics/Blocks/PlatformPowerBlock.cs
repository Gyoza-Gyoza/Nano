using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPowerBlock : MonoBehaviour
{
    [SerializeField]
    private PlatformBehaviour platform;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") platform.StartMove();
    }
}
