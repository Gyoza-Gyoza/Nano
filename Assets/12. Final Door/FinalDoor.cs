using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FinalDoor : MonoBehaviour
{
    [SerializeField]
    private Sprite[] doorUnlocks;

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraChannel;

    [SerializeField]
    private Animator fadeoutAnimator;

    [HideInInspector]
    public static FinalDoor instance;

    private SpriteRenderer spriteRenderer;
    private int doorsUnlocked; 
    public int DoorsUnlocked
    {  
        get { return doorsUnlocked; } 
        set 
        { 
            doorsUnlocked = value;
            //Play sound
            StartCoroutine(CameraShake(1f));
            CheckDoors();
        }
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        virtualCameraChannel = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private void Start()
    {
        CheckDoors();
    }
    private void CheckDoors()
    {
        spriteRenderer.sprite = doorUnlocks[doorsUnlocked];
    }
    private void OpenDoor()
    {
        DialogueManager.dialogueManager.StartDialogue(16);
        fadeoutAnimator.gameObject.SetActive(true);
        fadeoutAnimator.SetTrigger("Fadeout");
    }
    private IEnumerator CameraShake(float time)
    {
        virtualCameraChannel.m_AmplitudeGain = 1f;
        yield return new WaitForSeconds(time);
        virtualCameraChannel.m_AmplitudeGain = 0f;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (doorsUnlocked == 3) OpenDoor();
    }
}
