using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionGoal : Goal
{
    public int objectID;

    public CollectionGoal(int objectID,int goalID, string description, bool completed, int currentAmount, int requiredAmount)
    {
        this.objectID = objectID;
        this.goalID = goalID;
        this.description = description;
        this.completed = completed;
        this.currentAmount = currentAmount;
        this.requiredAmount = requiredAmount;
    }

    public override void Init()
    {
        base.Init();
        GameEvents.current.onCollectedOrDestroyedObject += CollectedItem;
    }

    public void CollectedItem(CollectableObject obj)
    {
        if (obj.id == this.objectID)
        {
            this.currentAmount++;
            Evaluate();
        }
    }
}
