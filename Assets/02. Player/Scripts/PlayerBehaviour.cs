using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private InputController input = null;

    [SerializeField] public float maxBattery = 100f;
    [SerializeField] public float currentBattery = 100f;
    [SerializeField] public float batteryDrainRate = 1f;

    private int drainCounter = 0;
    private Animator animator;
    private bool isDying = false;

    public BatteryBar batteryBar;
    public Vector2 respawnPos;

    #region Checkpoints
    private List<Vector3> checkpoints = new List<Vector3>();
    private Vector3 PreviousCheckpoint
    {   
        get
        {
            List<Vector3> positionsBehindPlayer = new List<Vector3>();

            foreach(Vector3 pos in checkpoints)
            {
                if(pos.x < transform.position.x) positionsBehindPlayer.Add(pos);
            }

            Tuple<Vector3, float>? target = null;

            foreach(Vector3 pos in positionsBehindPlayer)
            {
                if (target == null)
                    target = new Tuple<Vector3, float>(new Vector3(pos.x, pos.y, 0f), Vector3.Distance(transform.position, pos));

                else
                {
                    if (Vector3.Distance(transform.position, pos) < target.Item2)
                        target = new Tuple<Vector3, float>(new Vector3(pos.x, pos.y, 0f), Vector3.Distance(transform.position, pos));
                }
            }
            if (target != null)
            {
                return target.Item1;
            }
            else throw new InvalidOperationException("Nothing behind player");
        }
    }
    private Vector3 NextCheckpoint
    {
        get
        {
            List<Vector3> positionsAheadPlayer = new List<Vector3>();

            foreach (Vector3 pos in checkpoints)
            {
                if (pos.x > transform.position.x) positionsAheadPlayer.Add(pos);
            }

            Tuple<Vector3, float>? target = null;

            foreach (Vector3 pos in positionsAheadPlayer)
            {
                if (target == null)
                    target = new Tuple<Vector3, float>(new Vector3(pos.x, pos.y, 0f), Vector3.Distance(transform.position, pos));

                else
                {
                    if (Vector3.Distance(transform.position, pos) < target.Item2)
                        target = new Tuple<Vector3, float>(new Vector3(pos.x, pos.y, 0f), Vector3.Distance(transform.position, pos));
                }
            }
            if (target != null)
            {
                return target.Item1;
            }
            else throw new InvalidOperationException("Nothing in front of player");
        }
    }
    #endregion
    public BoxCollider2D col
    { get; private set; }
    public Rigidbody2D rb
    { get; private set; }

    public static PlayerBehaviour player; 

    private void Awake()
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
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        InitializeCheckpoints();
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

        if (Input.GetKeyDown(KeyCode.Z)) transform.position = PreviousCheckpoint;
        if (Input.GetKeyDown(KeyCode.X)) transform.position = NextCheckpoint;

        //This is a very janky fix, heavily dependent on unity's collision system.
        //If nothing happens, that's good, but please try to fix if you have time
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
        if(player.currentBattery <= 0)
        {
            if (isDying) return;
            PlayerDeath();
        }
    }

    public void DamageBattery()
    {
        currentBattery -= 10f;

        batteryBar.UpdateBattery();

        if(player.currentBattery <= 0)
        {
            if (isDying) return;
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
        StartCoroutine(PlayerRespawn(1f));
        GetComponent<Move>().MovementDisabled = true;
    }
    
    public IEnumerator PlayerRespawn(float duration)
    {
        animator.SetBool("isAlive", false);
        isDying = true;
        yield return new WaitForSeconds(duration);
        transform.position = respawnPos;
        animator.SetBool("isAlive", true);

        currentBattery = maxBattery;
        batteryBar.UpdateBattery();
        isDying = false;

        GetComponent<Move>().MovementDisabled = false;
    }

    public void UpdateRespawnPoint(Vector2 pos)
    {
        respawnPos = pos;
    }
    private void InitializeCheckpoints()
    {
        GameObject[] checkpointArray = GameObject.FindGameObjectsWithTag("Checkpoint");

        foreach (GameObject checkpoint in checkpointArray) checkpoints.Add(checkpoint.transform.position);
    }
}