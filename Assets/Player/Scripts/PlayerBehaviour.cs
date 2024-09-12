using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed,
        jumpHeight;

    [SerializeField]
    private int maxJumps;

    //Dylan
    [SerializeField]
    public float maxBattery = 100f,
    currentBattery = 100f,
    batteryDrainRate = 1f;

    public BatteryBar batteryBar;

    [SerializeField]
    private LayerMask groundMask;

    private int jumpCounter = 0;

    private Collider2D col; 
    private Rigidbody2D rb;

    public static PlayerBehaviour player; 
    private void Start()
    {
        if(player == null)
        {
            player = this;

            //Dylan
            currentBattery = maxBattery;
            batteryBar.SetMaxBattery();
        }
        else if (player != this)
        {
            Destroy(this);
        }

        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>(); 

        Debug.Log("Player Start Battery:" + currentBattery);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(-movementSpeed, rb.velocity.y); 
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(jumpCounter >= maxJumps)
            {
                return;
            }

            rb.AddForce(new Vector2(0f, jumpHeight));
            jumpCounter++; 
        }

        //Dylan
        if (Input.GetKeyDown(KeyCode.H))
        {
            ChargeBatteryTool();
        }

        if (Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - col.bounds.extents.y * 0.5f), 
            transform.localScale * 0.5f, 0f, Vector2.down, 0.01f, groundMask))
        {
            jumpCounter = 0;
        }
    }

    //Dylan
    public void DrainBattery()
    {
        currentBattery -= batteryDrainRate * Time.fixedDeltaTime; //Drain battery overtime
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery); //Ensures the battery doesnt exceed the maxBattery and 0.
        batteryBar.UpdateBattery(); //Update battery bar
    }
    
    //Dylan
    public void ChargeBatteryTool()
    {
        currentBattery += 10; //Add 10 everytime this function is called
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery); //Ensures the battery doesnt exceed the maxBattery and 0.
        batteryBar.UpdateBattery(); //Update battery bar
    }
}
