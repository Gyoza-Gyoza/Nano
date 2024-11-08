using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLogTrigger : MonoBehaviour
{
    [SerializeField]
    private int dataLogToTrigger;
    public bool unlocked = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(!unlocked) DataLogManager.instance.TriggerDataLog(dataLogToTrigger, this);
        }
    }
}
