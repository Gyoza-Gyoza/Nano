using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sunlight : MonoBehaviour
{
    private bool chargingPlayer = false;

    [SerializeField] 
    public float chargeRate = 26f;

    private void FixedUpdate()
    {
        if(chargingPlayer)
        {
            ChargeBattery();
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            chargingPlayer = true;
            PlayerBehaviour.player.healingVFX.Play();
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            chargingPlayer = false;
            PlayerBehaviour.player.healingVFX.Stop();
        }
    }

    public void ChargeBattery()
    {
        PlayerBehaviour.player.currentBattery += chargeRate * Time.fixedDeltaTime; //Charge battery overtime
        PlayerBehaviour.player.currentBattery = Mathf.Clamp(PlayerBehaviour.player.currentBattery, 0f, PlayerBehaviour.player.maxBattery); //Ensures the battery doesnt exceed the maxBattery and 0.
        PlayerBehaviour.player.batteryBar.UpdateBattery(); //Update battery bar
    }
}
