using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : Object
{
    public int id;
    public DialogueElement dialogue;

    private GameObject dialogueBox;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void Start()
    {
        dialogueBox = GameObject.FindGameObjectWithTag("DialogueBox");
    }

    public override void DisplayNextSentence()
    {
        dialogueBox.GetComponent<Dialogue>().DisplayNextSentence();
        GameEvents.current.TriggeredDialogue(id);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameEvents.current.TriggerDialogue(dialogue);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<Interactable>().enabled = false;
        }
    }
}
