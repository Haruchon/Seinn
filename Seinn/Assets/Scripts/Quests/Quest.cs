using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Quest : MonoBehaviour{

    public List<Goal> goals = new List<Goal>();

    public int id;
    public int questOrder;
    public string questTitle;
    public string questDescription;
    public bool completed;
    public int pointReward;

    public DialogueElement acceptDialogue;
    public DialogueElement declineDialogue;

    public GameObject triggers;

    private void Awake()
    {
        pointReward = 100;
        //Debug.Log(id + "has awaken");
    }

    public virtual void InitializeParameters() { }
    public virtual void InitializeObjects()
    {
        triggers.SetActive(true);
        GameEvents.current.onGoalCompletion += CheckGoals;
    }

    public void CheckGoals(Goal goal )
    {
        completed = goals.All(g => g.completed);
        // if(completed) achievements;
        if (completed)
            GameEvents.current.QuestCompleted(this);

    }

    public string GoalsToString()
    {
        string goalsToString = "";
        foreach(Goal goal in goals)
        {
            goalsToString += "\u2022<indent=3em>" + goal.description + "</indent>\n";
        }
        return goalsToString;
    }

    
}
