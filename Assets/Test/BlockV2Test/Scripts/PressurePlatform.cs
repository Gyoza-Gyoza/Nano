using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlatform : PlatformBlock
{
    private BoxCollider2D col;

    private bool isMoving = false;

    private float pos; 

    private void Start()
    {
        //col = GetComponent<BoxCollider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") isMoving = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") isMoving = false;
    }
    private void Update()
    {
        if (isMoving) Move();
        else Return();

        Debug.Log(pos);
    }
    public void Activate()
    {
        //No behaviour for activate 
    }
    private void Move()
    {
        Debug.Log("Moving");
        if(pos < 1f) pos += Time.deltaTime * speed;
        if(Vector3.Distance(transform.position, positions[1].position) >= 0.05f)
        {
            gameObject.transform.position = Vector3.Lerp(transform.position, positions[1].position, pos/1f);
        }
    }
    private void Return()
    {
        Debug.Log("Returning");
        if (pos > 0f) pos -= Time.deltaTime * speed;
        if (Vector3.Distance(transform.position, positions[0].position) >= 0.05f)
        {
            gameObject.transform.position = Vector3.Lerp(transform.position, positions[0].position, pos/1f);
        }
    }
}
