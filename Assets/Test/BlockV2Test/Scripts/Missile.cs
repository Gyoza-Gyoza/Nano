using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private float missileSpeed;
    public float MissileSpeed
    { get { return missileSpeed; } set { missileSpeed = value; } }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + missileSpeed, transform.position.z); 
    }
}
