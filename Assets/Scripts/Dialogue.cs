using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    public string DialogueId
    { get; private set; }

    public string DialogueSpeaker
    { get; private set; }

    public string DialogueText
    { get; private set; }

    public string DialogueAudio
    { get; private set; }

    public Dialogue(string dialogueId, string dialogueText, string dialogueAudio)
    {
        DialogueId = dialogueId;
        DialogueText = dialogueText;
        DialogueAudio = dialogueAudio;
    }
}
