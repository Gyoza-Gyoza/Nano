using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PowerBlockBehaviour : BlockBehaviour
{
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

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerOnBlock(this, true);

            Activate(new HashSet<BlockBehaviour>());
        }
    }
    //protected override void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player") SetBool(this, false);
    //}

    public void PlayerOnBlock(BlockBehaviour block, bool state)
    {
        Debug.Log($"Setting {block.gameObject.name} to {state}");
        connectionList[block] = state;

        CheckContacts();
    }
    private void CheckContacts()
    {
        bool allFalse = true;

        foreach (KeyValuePair<BlockBehaviour, bool> keyValuePair in connectionList) //Checks all the bools to see if there are any blocks that are being touched by the player 
        {
            Debug.Log($"Checking {keyValuePair.Key.gameObject.name}");
            if (keyValuePair.Value)
            {
                Debug.Log($"{keyValuePair.Key.gameObject.name} found");
                allFalse = false;
                break; //If its true, exit loop 
            }
        }

        if (allFalse)
        {
            Debug.Log("Deactivating time");
            Deactivate(new HashSet<BlockBehaviour>());
        }
    }
}
