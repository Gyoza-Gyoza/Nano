using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public virtual void Activate()
    {
        Debug.Log($"Activating {name}, this is the base Block class which has no behaviour, try to use one of the sub classes");
    }
    public virtual void Deactivate()
    {
        Debug.Log($"Deactivating {name}, this is the base Block class which has no behaviour, try to use one of the sub classes");
    }
}