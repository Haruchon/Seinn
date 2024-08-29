using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Threading;

public class ExtraDialogue : MonoBehaviour
{
    private const float TWEENING_TIME = 1f;
    private const float WAIT_TIME = 5f;
    private TextMeshProUGUI dialogueText;
    private Queue<string> sentences;
    private bool isShowing;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        isShowing = false;
        GameEvents.current.onShowQuestCompletion += ShowQuestCompletion;
        GameEvents.current.onShowCustomDialogue += ShowCustomNotification;
        dialogueText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        string str = null;
        if(sentences.Count > 0 && !isShowing)
        {
            str = sentences.Dequeue();
            if(str != null)
            {
                ShowExtraDialogue(str);
            }
        }
    }

    private IEnumerator HideDialogue()
    {
        yield return new WaitForSeconds(WAIT_TIME);
        LeanTween.moveLocalY(this.gameObject, 362f, TWEENING_TIME).setOnComplete(()=> { isShowing = false; });
        yield return new WaitForSeconds(TWEENING_TIME);
    }

    private IEnumerator ShowDialogue()
    {
        LeanTween.moveLocalY(this.gameObject, 242f, TWEENING_TIME);
        yield return null;
    }

    private void ShowExtraDialogue(string sentence)
    {
        dialogueText.text = "";
        StartCoroutine(ShowDialogue());
        StartCoroutine(TypeSentence(sentence));
        StartCoroutine(HideDialogue());
    }

    private void ShowCustomNotification(string sentence)
    {
        sentences.Enqueue(sentence);
    }

    private void ShowQuestCompletion(Quest quest)
    {
        sentences.Enqueue(quest.questTitle + " completado!\n Puntaje obtenido: " + quest.pointReward.ToString());
    }

    private IEnumerator TypeSentence(string sentence)
    {
        yield return new WaitForSeconds(TWEENING_TIME);
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

}
