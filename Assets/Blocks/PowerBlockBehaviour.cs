using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PowerBlockBehaviour : BlockBehaviour
{
    private bool drainingPlayer = false; 

    [SerializeField]
    private float timeBetweenChanges = 0.05f; 

    private Dictionary<BlockBehaviour, bool> connectionList = new Dictionary<BlockBehaviour, bool>(); //Contains a list of all connected blocks and a bool to check if blocks are being touched by the player

    private void Start()
    {
        currentState = BlockState.Power;

        GetConnections(this, connectionList);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (KeyValuePair<BlockBehaviour, bool> block in connectionList)
                Debug.Log(block.Key.gameObject.name);
        }
    }

    private void FixedUpdate()
    {
        if(drainingPlayer)
        {
            //Dylan
            PlayerBehaviour.player.DrainBattery();
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerOnBlock(this, true);

            StartCoroutine(Activate(new HashSet<BlockBehaviour>(), new WaitForSeconds(timeBetweenChanges)));
            drainingPlayer = true;
        }
    }
    protected override void OnCollisionExit2D(Collision2D collision)
    {
        drainingPlayer = false;
    }

    public void PlayerOnBlock(BlockBehaviour block, bool state)
    {
        connectionList[block] = state;

        CheckContacts();
    }
    private void CheckContacts()
    {
        bool allFalse = true;

        foreach (KeyValuePair<BlockBehaviour, bool> keyValuePair in connectionList) //Checks all the bools to see if there are any blocks that are being touched by the player 
        {
            if (keyValuePair.Value)
            {
                allFalse = false;
                break; //If its true, exit loop 
            }
        }

        if (allFalse)
        {
            StartCoroutine(Deactivate(new HashSet<BlockBehaviour>(), new WaitForSeconds(timeBetweenChanges)));
        }
    }
}
