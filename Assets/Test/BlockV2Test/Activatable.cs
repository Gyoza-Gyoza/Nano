using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activatable : MonoBehaviour
{
    public void Activate()
    {
        Debug.Log("This is the base activatable class which has no behaviour, try to use one of the sub classes");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if(this is PowerBlock powerBlock)
            {
                powerBlock.Activate(1);
            }
        }
    }
}