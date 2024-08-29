using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

//This is the dialogue manager
public class Dialogue : MonoBehaviour
{
    private const float TWEENING_TIME = 0.5f;
    private Queue<string> sentences = new Queue<string>();
    private bool hasQuest = false;
    private Quest currentQuest;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public GameObject dialogueBox;
    public GameObject questDialogueBox;

    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questDescription;
    public TextMeshProUGUI questGoals;

    private Animator anim;

    //public static bool gamePaused = false;   

    private void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        GameEvents.current.onTriggerDialogue += TriggerSomeDialogue;
    }

    public void TriggerSomeDialogue(DialogueElement dialogue)
    {
        StartDialogue(dialogue, false, null);
    }

    public void StartDialogue(DialogueElement dialogue, bool hasQuest, Quest quest)
    {
        sentences.Clear();
        this.hasQuest = hasQuest;
        this.currentQuest = quest;

        nameText.text = dialogue.NPC;

        FindObjectOfType<PlayerInteraction>().isDialogueActive = true;

        foreach (string sentence in dialogue.sentences)
            sentences.Enqueue(sentence);

        Active();
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            if (hasQuest)
                StartQuestDialogue();
            else
                EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach( char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    private IEnumerator TypeQuestSentence(string sentence)
    {
        questDescription.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            questDescription.text += letter;
            yield return null;
        }
    }

    private IEnumerator TypeQuestGoals(List<Goal> goals)
    {
        questGoals.text = "";
        foreach (Goal goal in goals)
        {
            questGoals.text += "\u2022<indent=3em>";
            foreach (char letter in goal.description.ToCharArray())
            {
                questGoals.text += letter;
                yield return null;
            }
            questGoals.text += "</indent>\n";
        }
    }

    public void EndDialogue()
    {
        Inactive();
        FindObjectOfType<PlayerInteraction>().isDialogueActive = false;
    }

    public void StartQuestDialogue()
    {
        GameEvents.current.DisableMovement();
        questTitle.text = currentQuest.questTitle;
        StartQuestDialogueTween();
        StopAllCoroutines();
        StartCoroutine(TypeQuestSentence(currentQuest.questDescription));
        StartCoroutine(TypeQuestGoals(currentQuest.goals));
        FindObjectOfType<PlayerInteraction>().isQuestDialogueActive = true;
        FindObjectOfType<PlayerInteraction>().isDialogueActive = false;
    }

    private void StartQuestDialogueTween()
    {
        GameEvents.current.DisableInteractions();
        Time.timeScale = 1f;
        LeanTween.moveLocalY(dialogueBox, -205f, TWEENING_TIME);
        LeanTween.moveLocalY(questDialogueBox, 0f, TWEENING_TIME).setDelay(TWEENING_TIME).setOnComplete(() => { Time.timeScale = 0f; GameEvents.current.EnableInteractions(); });
    }

    public void EndQuestDialogue()
    {
        GameEvents.current.EnableMovement();
        FindObjectOfType<PlayerInteraction>().isQuestDialogueActive = false;
        FindObjectOfType<PlayerInteraction>().isDialogueActive = false;
        EndQuestDialogueTween();
    }

    private void EndQuestDialogueTween()
    {
        GameEvents.current.DisableInteractions();
        Time.timeScale = 1f;
        LeanTween.moveLocalY(questDialogueBox, 500f, TWEENING_TIME).setOnComplete(() => { Time.timeScale = 0f; GameEvents.current.EnableInteractions(); });
    }

    public void Active()
    {
        //Debug.Log("Activando dialogo");
        GameEvents.current.DisableMovement();
        anim.SetBool("active", true);
        ActiveTween();
    }

    private void ActiveTween()
    {
        GameEvents.current.DisableInteractions();
        LeanTween.moveLocalY(dialogueBox, 0f, TWEENING_TIME).setOnComplete(() => { Time.timeScale = 0f; GameEvents.current.EnableInteractions(); });
    }

    public void Inactive()
    {
        //Debug.Log("Desactivando dialogo");
        GameEvents.current.EnableMovement();
        Time.timeScale = 1f;
        anim.SetBool("active", false);
        InactiveTween();
    }

    private void InactiveTween()
    {
        GameEvents.current.DisableInteractions();
        LeanTween.moveLocalY(dialogueBox, -205f, TWEENING_TIME).setOnComplete(()=> { GameEvents.current.EnableInteractions(); });
    }
}
