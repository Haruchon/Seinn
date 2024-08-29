using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    //Object Events 
    public event Action<CollectableObject> onCollectedOrDestroyedObject;
    public void CollectedOrDestroyedObject(CollectableObject obj)
    {
        onCollectedOrDestroyedObject?.Invoke(obj);
    }

    public event Action<string> onMissionGiven;
    public void MissionGiven(string str)
    {
        onMissionGiven?.Invoke(str);
    }

    public event Action<int> onCollectableMissionGiven;
    public void CollectableMissionGiven(int id)
    {
        onCollectableMissionGiven?.Invoke(id);
    }

    public event Action<int> onCollectableMissionFinished;
    public void CollectableMissionFinished(int id)
    {
        onCollectableMissionFinished?.Invoke(id);
    }

    //Enemy Events
    public event Action<Enemy> onEnemyDeath;
    public void EnemyDeath(Enemy enemy)
    {
        onEnemyDeath?.Invoke(enemy);
    }

    //Quest Events
    public event Action<Goal> onGoalCompletion;
    public void GoalCompletion(Goal goal)
    {
        onGoalCompletion?.Invoke(goal);
    }
    public event Action<Quest> onQuestCompleted;
    public void QuestCompleted(Quest quest)
    {
        onQuestCompleted?.Invoke(quest);
    }
    public event Action<Transform> onQuestAssign; //For quest pointer script
    public void QuestAssign(Transform transform)
    {
        onQuestAssign?.Invoke(transform);
    }
    public event Action<Quest> onShowQuestCompletion;
    public void ShowQuestCompletion(Quest quest)
    {
        onShowQuestCompletion?.Invoke(quest);
    }


    //Player Events
    public event Action<int> onUpdateHealth;
    public void UpdateHealth(int data)
    {
        onUpdateHealth?.Invoke(data);
    }
    public event Action<int> onUpdateScore;
    public void UpdateScore(int data)
    {
        onUpdateScore?.Invoke(data);
    }
    public event Action<int> onActivateHearts;
    public void ActivateHearts(int data)
    {
        onActivateHearts?.Invoke(data);
    }
    public event Action onDisableMovement;
    public void DisableMovement()
    {
        onDisableMovement?.Invoke();
    }
    public event Action onEnableMovement;
    public void EnableMovement()
    {
        onEnableMovement?.Invoke();
    }
    public event Action onDisableInteractions;
    public void DisableInteractions()
    {
        onDisableInteractions?.Invoke();
    }
    public event Action onEnableInteractions;
    public void EnableInteractions()
    {
        onEnableInteractions?.Invoke();
    }
    public event Action onFirstQuestAssign;
    public void FirstQuestAssign()
    {
        onFirstQuestAssign?.Invoke();
    }

    //Extra Dialogue Events
    public event Action<string> onShowCustomDialogue;
    public void ShowCustomDialogue(string str)
    {
        onShowCustomDialogue?.Invoke(str);
    }
    public event Action<DialogueElement> onTriggerDialogue;
    public void TriggerDialogue(DialogueElement dial)
    {
        onTriggerDialogue?.Invoke(dial);
    }
    public event Action<int> onTriggeredDialogue;
    public void TriggeredDialogue(int i)
    {
        onTriggeredDialogue?.Invoke(i);
    }
}
