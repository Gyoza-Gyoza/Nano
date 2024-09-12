using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform[] positions = new Transform[2];

    [SerializeField]
    private float liftDuration;

    private int currentPos = 0; 

    public void StartMove()
    {
        StartCoroutine(Move());
    }
    private IEnumerator Move()
    {
        float timer = 0;

        int endPos = currentPos == positions.Length - 1 ? 0 : currentPos + 1;

        Debug.Log($"currentPos is {currentPos} \n endPos is {endPos}");

        while (timer < liftDuration)
        {
            timer += Time.deltaTime;
            float t = timer / liftDuration; 
            gameObject.transform.position = Vector3.Lerp(positions[currentPos].position, positions[endPos].position, t);
            yield return null;
        }

        currentPos = endPos;
    }
}
