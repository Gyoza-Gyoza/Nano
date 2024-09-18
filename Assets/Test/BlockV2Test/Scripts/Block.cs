using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public virtual void Activate()
    {
        Debug.Log($"Activating {name}, this is the base ActivatableBlock class which has no behaviour, try to use one of the sub classes");
    }
}