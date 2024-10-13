using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Cinemachine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DialogueManager : MonoBehaviour
{   
    [Header("Camera Settings")]
    public CinemachineVirtualCamera virtualCamera;
    public float dialogueOrthoSize = 5f;
    public float normalOrthoSize = 8f;
    public float dialogueScreenY = 0.5f;
    public float normalScreenY = 0.75f;
    public float transitionDuration = 0.3f;
    private CinemachineFramingTransposer framingTransposer;

    [Header("Dialogue UI Settings")]
    public Animator textAnimator;
    public Animator potraitAnimator;
    public Image portraitImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameColor;
    public float textSpeed;
    public float delayBetweenSentence = 2f; 
    private Queue<string> sentences;
    private bool sentenceIsTyping = false;
    private string currentSentence;

    public static DialogueManager dialogueManager { get; private set; }

    void Awake()
    {
        if(dialogueManager != null & dialogueManager != this)
        {
            Destroy(gameObject);
            return;
        }

        dialogueManager = this;
    }
    void Start()
    {
        sentences = new Queue<string>();

        if (virtualCamera != null)
        {
            framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(sentenceIsTyping)
            {
                StopAllCoroutines(); // Stop typing the sentence
                dialogueText.text = currentSentence; // Show the full sentence
                sentenceIsTyping = false; // Mark that the sentence has been fully displayed 
            }
            else
            {
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        textAnimator.SetBool("isOpen", true);
        potraitAnimator.SetBool("isOpen", true);

        nameText.text = dialogue.name;
        portraitImage.sprite = dialogue.portrait;
        nameColor.color = dialogue.nameColor;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();

        // Adjust the camera for dialogue
        if (dialogue.changeCamera && virtualCamera != null)
        {
            // Change ortho size
            StartCoroutine(ChangeOrthoSize(virtualCamera, dialogueOrthoSize, transitionDuration));

            // Change Screen Y
            if (framingTransposer != null)
            {
                StartCoroutine(ChangeScreenY(framingTransposer, dialogueScreenY, transitionDuration));
            }
        }
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue(); // Store the current sentence
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence)); // Use currentSentence in the TypeSentence coroutine
    }

    IEnumerator TypeSentence(string sentence)
    {
        sentenceIsTyping = true;

        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        sentenceIsTyping = false;

        //Wait for specified delay before displaying next sentence
        yield return new WaitForSeconds(delayBetweenSentence);
        DisplayNextSentence();
    }

    public void EndDialogue()
    {
        textAnimator.SetBool("isOpen", false);
        potraitAnimator.SetBool("isOpen", false);
        
        if (virtualCamera != null)
        {
            // Reset ortho size
            StartCoroutine(ChangeOrthoSize(virtualCamera, normalOrthoSize, transitionDuration));

            // Reset Screen Y
            if (framingTransposer != null)
            {
                StartCoroutine(ChangeScreenY(framingTransposer, normalScreenY, transitionDuration));
            }
        }
    }

    private IEnumerator ChangeOrthoSize(CinemachineVirtualCamera cam, float targetSize, float duration)
    {
        float startSize = cam.m_Lens.OrthographicSize;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            t = SlowEaseInOut(t);

            cam.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.m_Lens.OrthographicSize = targetSize;
    }

    private IEnumerator ChangeScreenY(CinemachineFramingTransposer transposer, float targetY, float duration)
    {
        float startY = transposer.m_ScreenY;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            t = SlowEaseInOut(t);

            transposer.m_ScreenY = Mathf.Lerp(startY, targetY, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transposer.m_ScreenY = targetY;
    }

    private float SlowEaseInOut(float t)
    {
        return Mathf.Pow(t, 3) * (t * (t * 6 - 15) + 10);
    }

}

