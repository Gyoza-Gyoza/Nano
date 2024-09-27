using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PowerBlock : Block
{
    private Dictionary<Block, bool> connections = new Dictionary<Block, bool>();
    public Dictionary<Block, bool> Connections
    { get { return connections; } }

    private void Start()
    {
        connections.Add(this, false);
        foreach (Block block in neighbours) //Checks through neighbours 
        {
            if (block is BridgingBlock) //Checks if its a bridging block 
            {
                BridgingBlock bridgingBlock = block as BridgingBlock; //Casts it as a bridging block 
                bridgingBlock.InitializeConnections(this, connections); 
            }
        }    
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            foreach (KeyValuePair<Block, bool> kvp in Connections)
            {
                Debug.Log($"{kvp.Key.name}");
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isDraining = true;
            PlayerOnBlock(this, true);
            Activate();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isDraining = false;
            PlayerOnBlock(this, false);
            Deactivate();
        }
    }
    public override void Activate()
    {
        foreach(Block block in neighbours)
        {
            block.Charge();
        }
    }
    public override void Deactivate()
    {
        foreach (Block block in neighbours)
        {
            if (block is BridgingBlock) continue;
            block.Discharge();
        }
    }
    #region BridgingBlock Functions
    public void PlayerOnBlock(Block block, bool state)
    {
        connections[block] = state;

        CheckContacts();
    }
    private void CheckContacts()
    {
        foreach (KeyValuePair<Block, bool> keyValuePair in connections) //Checks all the bools to see if there are any blocks that are being touched by the player 
        {
            if (keyValuePair.Value) //If a true is found, exit function
            {
                return; 
            }
        }
        //If loop completes, means there are no trues and discharges the blocks
        foreach (Block block in neighbours)
        {
            if (block is BridgingBlock) block.Discharge();
        }
    }
    #endregion
}
