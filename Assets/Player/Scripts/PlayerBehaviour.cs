using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    //[SerializeField] private float movementSpeed, jumpHeight;
    //[SerializeField] private int maxJumps;
    //[SerializeField] private LayerMask groundMask;
    //private int jumpCounter = 0;

    [SerializeField] private InputController input = null;

    [SerializeField] public float maxBattery = 100f;
    [SerializeField] public float currentBattery = 100f;
    [SerializeField] public float batteryDrainRate = 1f;

    public BatteryBar batteryBar;
    public Vector2 respawnPos;

    private BoxCollider2D col; 
    private Rigidbody2D rb;

    public static PlayerBehaviour player; 

    private void Start()
    {
        if(player == null)
        {
            player = this;

            respawnPos = transform.position;
            currentBattery = maxBattery;
            batteryBar.SetMaxBattery();
        }

        else if (player != this)
        {
            Destroy(this);
        }

        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>(); 
    }

    private void Update()
    {
        if (input.RetrieveHealInput())
        {
            ChargeBatteryTool();
        }
        
        if (input.RetrieveRespawnInput())
        {
            PlayerDeath();
        }

        /*
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

        if (Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - col.bounds.extents.y * 0.5f), transform.localScale * 0.5f, 0f, Vector2.down, 0.01f, groundMask))
        {
            jumpCounter = 0;
        }
        */
    }

    public void DrainBattery()
    {
        currentBattery -= batteryDrainRate * Time.fixedDeltaTime; //Drain battery overtime
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery); //Ensures the battery doesnt exceed the maxBattery and 0.

        batteryBar.UpdateBattery(); //Update battery bar

        //If player battery reaches 0 then call death function
        if(player.currentBattery == 0)
        {
            PlayerDeath();
        }
    }
    
    public void ChargeBatteryTool()
    {
        currentBattery += 10; //Add 10 everytime this function is called
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery); //Ensures the battery doesnt exceed the maxBattery and 0.
        batteryBar.UpdateBattery();
    }

    public void PlayerDeath()
    {
        StartCoroutine(PlayerRespawn(0.1f));
    }
    
    public IEnumerator PlayerRespawn(float duration)
    {
        yield return new WaitForSeconds(duration);
        transform.position = respawnPos;   

        currentBattery = maxBattery;
        batteryBar.UpdateBattery();
    }

    public void UpdateRespawnPoint(Vector2 pos)
    {
        respawnPos = pos;
    }
}
