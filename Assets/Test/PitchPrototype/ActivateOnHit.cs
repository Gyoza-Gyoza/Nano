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

    public int bridgeCount = 0;

    public bool playerOnTiles = false, 
        bridgeActive = false, 
        playerOnBridge = false;

    private void Start()
    {
        foreach(ActivateBlock block in tilesToActivate)
        {
            block.mainBlock = this;
        }
    }
    private void Update()
    {
        Debug.Log(bridgeCount);
        if (bridgeActive && !playerOnTiles && bridgeCount == 0)
        {
            bridgeActive = false;
            StartCoroutine(DeactivateTiles());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerOnTiles = true;
        bridgeCount++;
        StartCoroutine(ActivateTiles());
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        bridgeCount--;
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
