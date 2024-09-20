using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBlock : Block
{
    [SerializeField]
    protected Transform[] positions = new Transform[2];

    [SerializeField]
    protected float speed;
}
