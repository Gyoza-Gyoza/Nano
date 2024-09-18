using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private BoxCollider2D col; 

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    public void TriggerDialogue()
    {
        DialogueManager.dialogueManager.StartDialogue(dialogue);
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            col.enabled = false;
            TriggerDialogue();
        }
    }

    private void OnTriggerExit2D (Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            col.enabled = false;
        }
    }
}
