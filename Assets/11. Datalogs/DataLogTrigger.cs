using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLogTrigger : MonoBehaviour
{
    [SerializeField]
    private int dataLogToTrigger;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            DataLogManager.instance.TriggerDataLog(dataLogToTrigger);
        }
    }
}
