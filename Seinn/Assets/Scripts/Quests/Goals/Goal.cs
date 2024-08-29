using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal
{
    public int goalID;
    public bool completed;
    public string description;
    public int currentAmount;
    public int requiredAmount;

    public virtual void Init()
    {
        //default init
    }
    public void Evaluate()
    {
        if (currentAmount >= requiredAmount)
            Complete();
    }
    public void Complete()
    {
        completed = true;
        GameEvents.current.GoalCompletion(this);
    }

}

