using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TogglePlatform : PlatformBlock
{
    protected int currentPos = 0;
    public override void Activate()
    {
        StartCoroutine(Move());
    }
    protected IEnumerator Move()
    {
        float timer = 0;

        int endPos = currentPos == positions.Length - 1 ? 0 : currentPos + 1;

        while (timer < 1f)
        {
            timer += Time.deltaTime * speed;
            float t = timer / 1f;
            gameObject.transform.position = Vector3.Lerp(positions[currentPos].position, positions[endPos].position, t);
            yield return null;
        }

        currentPos = endPos;
    }
}
