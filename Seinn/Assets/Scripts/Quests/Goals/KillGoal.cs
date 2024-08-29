using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGoal : Goal
{
    public int enemyID;

    public KillGoal(int enemyID, int goalID, string description, bool completed, int currentAmount, int requiredAmount)
    {
        this.enemyID = enemyID;
        this.goalID = goalID;
        this.description = description;
        this.completed = completed;
        this.currentAmount = currentAmount;
        this.requiredAmount = requiredAmount;
    }

    public override void Init()
    {
        base.Init();
        GameEvents.current.onEnemyDeath += enemyDied; // Add enemy died to combat events?
    }

    public void enemyDied(Enemy enemy)
    {
        if(enemy.id == this.enemyID)
        {
            this.currentAmount++;
            Evaluate();
        }
    }
}
