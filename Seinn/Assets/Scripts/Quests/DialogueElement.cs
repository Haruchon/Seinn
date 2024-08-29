using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueElement
{
    public string NPC;

    [TextArea(3, 10)]
    public string[] sentences;

    public DialogueElement(string npc, string[] sentences)
    {
        this.NPC = npc;
        this.sentences = sentences;
    }
}
