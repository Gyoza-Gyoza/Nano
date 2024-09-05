using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnHit : MonoBehaviour
{
    [SerializeField]
    private ActivateBlock[] tilesToActivate;

    [SerializeField]
    private Color activateColor,
        inactiveColor; 

    [SerializeField]
    private float timeToChange = 0.05f;

    public bool playerOnTiles = false, 
        bridgeActive = false;

    private void Update()
    {
        if(bridgeActive && !playerOnTiles)
        {
            bridgeActive = false;
            StartCoroutine(DeactivateTiles());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerOnTiles = true;
        StartCoroutine(ActivateTiles());
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        playerOnTiles = false;
    }
    private IEnumerator ActivateTiles()
    {
        foreach (ActivateBlock tile in tilesToActivate)
        {
            yield return new WaitForSeconds(timeToChange);
            tile.ActivateTile(activateColor);
            bridgeActive = true;
        }
    }
    private IEnumerator DeactivateTiles()
    {
        foreach (ActivateBlock tile in tilesToActivate)
        {
            yield return new WaitForSeconds(timeToChange);
            tile.DeactivateTile(inactiveColor);
        }
    }
}
