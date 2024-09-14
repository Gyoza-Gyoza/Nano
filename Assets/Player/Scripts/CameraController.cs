using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void Update()
    {
        gameObject.transform.position = new Vector3(PlayerBehaviour.player.transform.position.x, PlayerBehaviour.player.transform.position.y, transform.position.z);
    }
}
