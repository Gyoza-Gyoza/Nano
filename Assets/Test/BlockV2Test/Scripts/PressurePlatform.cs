using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlatform : PlatformBlock
{
    private BoxCollider2D col;

    private bool ascending = false;

    [Range(0f, 1f)]
    public float currentPos; 

    private void Start()
    {
        //col = GetComponent<BoxCollider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") ascending = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") ascending = false;
    }
    private void Update()
    {
        Move();
    }
    public override void Activate()
    {
        //No behaviour for activate 
    }
    private void Move()
    {
        Debug.Log(currentPos);
        float timeChange = ascending ? Time.deltaTime * speed : -Time.deltaTime * speed;
        currentPos = Mathf.Clamp(currentPos + timeChange, 0f, 1f);

        if (Vector3.Distance(transform.position, positions[1].position) <= 0.05f && ascending) 
            transform.position = positions[1].position;

        else if (Vector3.Distance(transform.position, positions[0].position) <= 0.05f && !ascending)
            transform.position = positions[0].position;

        else 
            transform.position = Vector3.Lerp(positions[0].position, positions[1].position, currentPos);
    }
    private void Return()
    {
        Debug.Log(currentPos);
        currentPos = Mathf.Clamp(currentPos - Time.deltaTime * speed, 0f, 1f);

        if (Vector3.Distance(transform.position, positions[0].position) <= 0.05f)
        {
            transform.position = positions[0].position;
        }
        else
        {
            transform.position = Vector3.Lerp(positions[0].position, positions[1].position, currentPos);
        }
    }
}
