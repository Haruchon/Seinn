using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager current;
    private const float TWEENING_TIME = 0.5f;
    public List<Quest> quests = new List<Quest>();
    public List<GameObject> questLocations;

    public List<GameObject> questGivers = new List<GameObject>();

    public int activeQuest;
    public int nextQuest;

    public GameObject questDialogueBox;
    public GameObject acceptButton;
    public GameObject declineButton;
    public GameObject continueButton;

    private void Awake()
    {
        current = this;
        GameEvents.current.onQuestCompleted += CurrentQuestCompleted;
    }

    public void CurrentQuestCompleted(Quest quest)
    {
        if(activeQuest != -1 && activeQuest == quest.questOrder)
        {
            GameEvents.current.ShowQuestCompletion(quests[activeQuest - 1]);
            GameEvents.current.UpdateScore(quests[activeQuest - 1].pointReward);
            activeQuest = -1;
        }
    }

    public void AssignQuest(int questOrder)
    {
        activeQuest = questOrder;
        if (questOrder == 1) ShowFirstWeapon();
        quests[activeQuest - 1].InitializeObjects();
        GameEvents.current.QuestAssign(questLocations[activeQuest - 1].transform);
        SetNextQuest();
    }

    private void ShowFirstWeapon()
    {
        GameEvents.current.FirstQuestAssign();
    }

    void SetNextQuest()
    {
        if (nextQuest <= 11)
            nextQuest++;
        else
            nextQuest = -1;
    }

    private void LoadQuestData()
    {
        if (activeQuest != -1)
        {
            questDialogueBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = quests[activeQuest-1].questTitle;
            questDialogueBox.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = quests[activeQuest-1].questDescription;
            questDialogueBox.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = quests[activeQuest-1].GoalsToString();

        }
        else
        {
            questDialogueBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Sin Mision";
            questDialogueBox.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "No se tiene una mision activa";
            questDialogueBox.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "\u2022<indent=3em>Sin objetivos</indent>";
        }
            
    }

    public void ShowQuestLog()
    {
        LoadQuestData();
        ShowContinueButton();
        LeanTween.moveLocalY(questDialogueBox, 0f, TWEENING_TIME);
    }

    private void ShowContinueButton()
    {
        acceptButton.SetActive(false);
        declineButton.SetActive(false);
        continueButton.SetActive(true);
    }

    public void HideQuestLog()
    {
        LeanTween.moveLocalY(questDialogueBox, 500f, TWEENING_TIME).setOnComplete(HideContinueButton);
    }

    private void HideContinueButton()
    {
        acceptButton.SetActive(true);
        declineButton.SetActive(true);
        continueButton.SetActive(false);
    }
}
