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
    private Queue<Dialogue> sentences = new Queue<Dialogue>();
    private bool sentenceIsTyping = false, zoomedIn = false;
    private Dialogue currentDialogue;
    private string currentSentence;
    [SerializeField]
    private float dialogueDelay;
    [SerializeField]
    private AudioSource voiceoverSource;

    public static DialogueManager dialogueManager { get; private set; }

    [SerializeField]
    private List<Speakers> speakers = new List<Speakers>();

    private WaitForSeconds dialogueDelayTime, typingSpeed;

    void Awake()
    {
        Database.InitializeDatabases();
        if(dialogueManager != null & dialogueManager != this)
        {
            Destroy(gameObject);
            return;
        }

        dialogueManager = this;
    }
    void Start()
    {
        if (virtualCamera != null)
        {
            framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        dialogueDelayTime = new WaitForSeconds(dialogueDelay);
        typingSpeed = new WaitForSeconds(textSpeed);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(sentenceIsTyping && voiceoverSource.isPlaying)
            {
                StopAllCoroutines(); // Stop typing the sentence
                dialogueText.text = currentSentence; // Show the full sentence
                sentenceIsTyping = false; // Mark that the sentence has been fully displayed 
                voiceoverSource.Stop();
            }
            else
            {
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue(int chosenDialogue)
    {
        textAnimator.SetBool("isOpen", true);
        potraitAnimator.SetBool("isOpen", true);

        //nameText.text = dialogue.name;
        //portraitImage.sprite = dialogue.portrait;
        //nameColor.color = dialogue.nameColor;

        sentences.Clear();

        //foreach(string sentence in dialogue.sentences)
        //{
        //    sentences.Enqueue(sentence);
        //}

        foreach(Dialogue dialogue in Database.DialogueDatabase[chosenDialogue])
        {
            sentences.Enqueue(dialogue);
        }

        DisplayNextSentence();

        // Adjust the camera for dialogue
        //if (dialogue.changeCamera && virtualCamera != null)
        //{
        //    // Change ortho size
        //    StartCoroutine(ChangeOrthoSize(virtualCamera, dialogueOrthoSize, transitionDuration));

        //    // Change Screen Y
        //    if (framingTransposer != null)
        //    {
        //        StartCoroutine(ChangeScreenY(framingTransposer, dialogueScreenY, transitionDuration));
        //    }
        //}
    }
    public void DisplayNextSentence()
    {
        voiceoverSource.Stop();
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        currentDialogue = sentences.Dequeue();

        //Find current speaker
        foreach (Speakers speakers in speakers)
        {
            if (currentDialogue.DialogueSpeaker == speakers.speaker)
            {
                portraitImage.sprite = speakers.portrait;
                voiceoverSource.clip = currentDialogue.DialogueAudio;
                voiceoverSource.Play();
            }
        }
        currentSentence = currentDialogue.DialogueText; // Store the current sentence

        //StopAllCoroutines();

        if (currentDialogue.Cutscene)
        {
            Move.instance.MovementDisabled = true;
            ZoomIn();
        }
        else ZoomOut();

        StartCoroutine(TypeSentence(currentSentence)); // Use currentSentence in the TypeSentence coroutine
    }

    IEnumerator TypeSentence(string sentence)
    {
        sentenceIsTyping = true;

        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return typingSpeed;
        }

        sentenceIsTyping = false;

        //Wait for specified delay before displaying next sentence
        yield return new WaitUntil(() => !voiceoverSource.isPlaying);
        yield return dialogueDelayTime;
        DisplayNextSentence();
    }

    public void EndDialogue()
    {
        textAnimator.SetBool("isOpen", false);
        potraitAnimator.SetBool("isOpen", false);
        Move.instance.MovementDisabled = false;

        //if (virtualCamera != null)
        //{
        //    // Reset ortho size
        //    StartCoroutine(ChangeOrthoSize(virtualCamera, normalOrthoSize, transitionDuration));

        //    // Reset Screen Y
        //    if (framingTransposer != null)
        //    {
        //        StartCoroutine(ChangeScreenY(framingTransposer, normalScreenY, transitionDuration));
        //    }
        //}
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
    private void ZoomIn()
    {
        if (zoomedIn) return;

        zoomedIn = true;

        // Change ortho size
        StartCoroutine(ChangeOrthoSize(virtualCamera, dialogueOrthoSize, transitionDuration));

        // Change Screen Y

        StartCoroutine(ChangeScreenY(framingTransposer, dialogueScreenY, transitionDuration));
    }
    private void ZoomOut()
    {
        StartCoroutine(ChangeOrthoSize(virtualCamera, normalOrthoSize, transitionDuration));

        // Reset Screen Y

        StartCoroutine(ChangeScreenY(framingTransposer, normalScreenY, transitionDuration));

        zoomedIn = false;
    }
}

[System.Serializable]
public class Dialogue
{
    public string DialogueId
    { get; private set; }

    public Speaker DialogueSpeaker
    { get; private set; }

    public string DialogueText
    { get; private set; }

    public AudioClip DialogueAudio
    { get; private set; }

    public bool Cutscene
    { get; private set; }
    public Dialogue(string dialogueId, string dialogueSpeaker, string dialogueText, string dialogueAudio, string cutscene)
    {
        DialogueId = dialogueId;
        DialogueSpeaker = (Speaker) Enum.Parse(typeof(Speaker), dialogueSpeaker);
        DialogueText = dialogueText.Replace('@', ',');
        AssetManager.LoadVoiceover(dialogueAudio, (AudioClip aud) => DialogueAudio = aud);
        Cutscene = cutscene == "TRUE" ? true : false;
    }
}

public enum Speaker
{
    MissionControl,
    RogueAI
}

[System.Serializable]
public class Speakers
{
    public Speaker speaker; 
    public Sprite portrait;
}