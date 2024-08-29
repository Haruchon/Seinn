using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Interactable : Object
{
    public DialogueElement dialogue;
    public DialogueElement questAssignedDialogue;
    public DialogueElement postQuestDialogue;

    private DialogueElement cantGiveQuestDialogue;
    private DialogueElement hasQuestActiveDialogue;

    private GameObject dialogueBox;

    public int questOrder = 0;

    [SerializeField]
    private Quest quest;

    public bool hasQuest = false;
    public bool questAssigned = false;
    public bool completedQuest = false;

    private void Start()
    {
        dialogueBox = GameObject.FindGameObjectWithTag("DialogueBox");
        if (hasQuest)
        {
            GameEvents.current.onQuestCompleted += QuestCompleted;
            quest = GameObject.FindObjectOfType<QuestManager>().quests[questOrder - 1].GetComponent<Quest>();
        }

        cantGiveQuestDialogue = new DialogueElement(dialogue.NPC, new string[] { "Debes completar primero las misiones: " + QuestNeeded(questOrder) });
        hasQuestActiveDialogue = new DialogueElement(dialogue.NPC, new string[] { "Ya tienes una misión activa en este momento. \nVuelve cuando completes dicha mision." });
    }

    private string QuestNeeded(int questOrder)
    {
        if (questOrder <= 2)
            return "1";
        else
            return QuestNeeded(questOrder - 1) + ", " + (questOrder-1).ToString() ;
    }

    public void QuestCompleted(Quest quest)
    {
        if (hasQuest )
        {
            if (quest.id == this.quest.id)
                completedQuest = true;
        }
    }

    public override void StartDialogue()
    {
        if (completedQuest)
            dialogueBox.GetComponent<Dialogue>().StartDialogue(postQuestDialogue, false, null); 
        else if (questAssigned)
            dialogueBox.GetComponent<Dialogue>().StartDialogue(questAssignedDialogue, false, null);
        else
            dialogueBox.GetComponent<Dialogue>().StartDialogue(dialogue, hasQuest, quest);
    }

    public override void DisplayNextSentence()
    {
        dialogueBox.GetComponent<Dialogue>().DisplayNextSentence();
    }

    public override void AcceptDialogue()
    {
        dialogueBox.GetComponent<Dialogue>().EndQuestDialogue();
        if(QuestManager.current.activeQuest == -1)
        {
            if (QuestManager.current.nextQuest == this.questOrder)
            {
                dialogueBox.GetComponent<Dialogue>().StartDialogue(quest.acceptDialogue, false, null);
                GameObject.FindObjectOfType<QuestManager>().AssignQuest(questOrder);
                questAssigned = true;
            }
            else
            {
                dialogueBox.GetComponent<Dialogue>().StartDialogue(cantGiveQuestDialogue, false, null);
            }
        }
        else
        {
            dialogueBox.GetComponent<Dialogue>().StartDialogue(hasQuestActiveDialogue, false, null);
        }
    }

    public override void DeclineDialogue()
    {
        dialogueBox.GetComponent<Dialogue>().EndQuestDialogue();
        dialogueBox.GetComponent<Dialogue>().StartDialogue(quest.declineDialogue, false, null);
    }
}
