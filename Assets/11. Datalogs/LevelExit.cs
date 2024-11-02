using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField]
    private string exitScene;
    private AudioSource audioSource;
    [SerializeField]
    private int doors = 3;
    [SerializeField]
    private float cameraShakeDuration = 1f; 
    [SerializeField]
    private AudioClip unlockDoorSFX, lockedDoorSFX, openDoorSFX;

    private bool playerInRange = false;
    private CinemachineBasicMultiChannelPerlin virtualCamera;

    private int doorsUnlocked;
    public int DoorsUnlocked
    {
        get { return doorsUnlocked; }
        set 
        {
            doorsUnlocked = value;
            UnlockDoor();
        }
    }
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerInRange && DoorsUnlocked == doors) 
            SceneManager.LoadScene(exitScene);

        if (Input.GetKeyDown(KeyCode.Q)) DoorsUnlocked++;
    }
    private void UnlockDoor() //Called when completing a datalog
    {
        //Play sound
        SwapAudio(unlockDoorSFX);

        StartCoroutine(CameraShake());

        //Maybe make indicators to show how many doors are unlocked 
        //and change the indicators 
    }
    private IEnumerator CameraShake()
    {
        virtualCamera.m_AmplitudeGain = 1f;
        yield return new WaitForSeconds(cameraShakeDuration);
        virtualCamera.m_AmplitudeGain = 0f;
    }
    public void OpenDoor()
    {
        if (DoorsUnlocked == doors) SwapAudio(openDoorSFX);
        else SwapAudio(lockedDoorSFX);
    }
    private void SwapAudio(AudioClip audio)
    {
        audioSource.clip = audio; 
        audioSource.Play();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerInRange = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerInRange = false;
    }
}