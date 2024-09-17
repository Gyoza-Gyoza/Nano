using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBlock : Activatable
{
    private HashSet<Activatable> activatables = new HashSet<Activatable>();
    public HashSet<Activatable> Activatables
    { 
        get { return activatables; }
    }
    public void Activate(int number)
    {
        Debug.Log(number);
    }
    public void AddActivatables(Activatable activatableToAdd)
    {
        if(!activatables.Contains(activatableToAdd))
        {
            activatables.Add(activatableToAdd);
        }
    }
}
