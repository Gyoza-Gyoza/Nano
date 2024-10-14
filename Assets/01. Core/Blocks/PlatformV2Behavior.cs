using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformV2Behavior : MonoBehaviour
{
    [SerializeField]
    private Transform[] positions = new Transform[2];

    [SerializeField]
    private float liftSpeed;

    private int currentPos = 0;

    private void Update()
    {
        
    }

    //private void Move()
    //{
    //    float timer = 0;

    //    int endPos = currentPos == positions.Length - 1 ? 0 : currentPos + 1;

    //    Debug.Log($"currentPos is {currentPos} \n endPos is {endPos}");

    //    if (timer < liftDuration)
    //    {
    //        timer += Time.deltaTime;
    //        float t = timer / liftDuration;
    //        gameObject.transform.position = Vector3.Lerp(positions[currentPos].position, positions[endPos].position, t);
    //    }

    //    currentPos = endPos;
    //}
}
