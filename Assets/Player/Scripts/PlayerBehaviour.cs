using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private InputController input = null;

    [SerializeField] public float maxBattery = 100f;
    [SerializeField] public float currentBattery = 100f;
    [SerializeField] public float batteryDrainRate = 1f;

    [SerializeField]
    public ParticleSystem healingVFX;

    private int drainCounter = 0;

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

        //This is a very janky fix, if nothing happens, that's good, but please try to fix if you have time
        if (drainCounter > 0) DrainBattery(); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Block>() != null) drainCounter++;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Block>() != null) drainCounter--;
    }

    public void DrainBattery()
    {
        currentBattery -= batteryDrainRate; //Drain battery overtime
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
