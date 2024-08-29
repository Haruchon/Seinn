using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : Object
{
    private const float RANGE_MIN_NUMBER = -100f;
    private const float RANGE_MAX_NUMBER = 100f;

    private const int CORRECT_EXP = 10;
    private const int INCORRECT_EXP = -5;

    public int questOrder;
    public int id;
    public int operation = 0;
    public double value;
    [SerializeField]
    private bool canGiveMission;
    [SerializeField]
    private bool missionGiven;
    [SerializeField]
    private bool hasMission;
    [SerializeField]
    private bool missionFinished;
    [SerializeField]
    private bool interactable;
    public DialogueElement dialogue;
    public DialogueElement extraDialogue;
    public DialogueElement correctDialogue;
    public DialogueElement incorrectDialogue;

    private DialogueElement cantGiveMissionDialogue;
    private GameObject numberSpawnPoints;
    private GameObject randomNumberPrefab;

    private GameObject dialogueBox;
    private Weapon weapon;
    private System.Random seed;

    public void EnableInteractions()
    {
        this.interactable = true;
    }

    public void DisableInteractions()
    {
        this.interactable = false;
    }

    public void InitializeData(int operation, double value,bool hasMission, List<DialogueElement> dialogues, int questOrder)
    {
        this.operation = operation;
        this.value = value;
        this.hasMission = hasMission;
        this.questOrder = questOrder;
        this.dialogue = dialogues[0];
        this.extraDialogue = dialogues[1];
        this.correctDialogue = dialogues[2];
        this.incorrectDialogue = dialogues[3];
    }

    private void Start()
    {
        missionFinished = false;
        missionGiven = false;
        canGiveMission = true;
        seed = new System.Random();
        dialogueBox = GameObject.FindGameObjectWithTag("DialogueBox");
        weapon = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Weapon>();
        GameEvents.current.onCollectableMissionGiven += LockMissions;
        GameEvents.current.onCollectableMissionFinished += ReleaseMissions;
        cantGiveMissionDialogue = new DialogueElement(dialogue.NPC, new string[] { "¡Debes terminar lo que ya haz empezado!!" }); 
    }

    private void LockMissions(int id)
    {
        if(id == this.id && !this.missionGiven && !this.missionFinished)
        {
            canGiveMission = false;
        }
    }

    private void ReleaseMissions(int id)
    {
        if (id == this.id && !this.missionGiven && !this.missionFinished)
        {
            canGiveMission = true;
        }
    }

    private void CheckSpawns()
    {
        bool needSpawn = false;
        foreach(Transform child in numberSpawnPoints.transform)
        {
            if(child.childCount < 0)
            {
                needSpawn = true;
            }
        }
        if (needSpawn) SpawnNumber();
    }

    private void DestroyNumbers()
    {
        if(numberSpawnPoints)
            foreach(Transform child in numberSpawnPoints.transform)
            {
                if(child.childCount > 0)
                    Destroy(child.GetChild(0).gameObject);
            }
    }

    private void SpawnNumber()
    {
        bool isSpawnedSolution = false;
        CustomNumber numberToSpawn;
        int operationToUse = 0;
        for (int i = 0; i < numberSpawnPoints.transform.childCount; i++)
        {
            Transform spawn = numberSpawnPoints.transform.GetChild(i);
            numberToSpawn = NumberConfig.current.GetNewNumber(questOrder);
            if (!isSpawnedSolution)
            {
                operationToUse = CustomNumber.GetOperation(questOrder);
                switch (operationToUse)
                {

                    case 0:
                        numberToSpawn = new CustomNumber(operationToUse, value - weapon.bulletDmg[operationToUse],NumberConfig.current.GetNumberType(questOrder));
                        break;

                    case 1:
                        numberToSpawn = new CustomNumber(operationToUse, value + weapon.bulletDmg[operationToUse], NumberConfig.current.GetNumberType(questOrder));
                        break;

                    case 2:
                        numberToSpawn = new CustomNumber(operationToUse, value / weapon.bulletDmg[operationToUse], NumberConfig.current.GetNumberType(questOrder));
                        break;

                    case 3:
                        numberToSpawn = new CustomNumber(operationToUse, value * weapon.bulletDmg[operationToUse], NumberConfig.current.GetNumberType(questOrder));
                        break;

                    default:
                        Debug.Log("Algo malo pasó en la creacion de un numero blanco");
                        break;
                }
                isSpawnedSolution = true;
            }
            GameObject randomNumber = Instantiate(randomNumberPrefab, spawn.position, Quaternion.identity, spawn) as GameObject;
            randomNumber.GetComponent<Number>().value = numberToSpawn;
        }
    }

    public void SetSpawner(GameObject spawnPoints, GameObject prefab)
    {
        numberSpawnPoints = spawnPoints;
        randomNumberPrefab = prefab;
    }

    private void GiveReward()
    {
        dialogueBox.GetComponent<Dialogue>().StartDialogue(correctDialogue, false, null);
        missionFinished = true;
        //Destroy(gameObject);
    }

    private void CheckCompletion()
    {
        
        if (Utils.CheckEquality(weapon.bulletDmg[operation],value))
        {
            GameEvents.current.UpdateScore(CORRECT_EXP);
            GiveReward();
        }
        else
        {
            dialogueBox.GetComponent<Dialogue>().StartDialogue(incorrectDialogue, false, null);
            GameEvents.current.UpdateScore(INCORRECT_EXP);
            DestroyNumbers();
            SpawnNumber();
        }

    }

    public override void StartDialogue()
    {
        if (interactable)
        {
            if (hasMission)
            {
                if (canGiveMission)
                {
                    if (!missionGiven)
                        dialogueBox.GetComponent<Dialogue>().StartDialogue(dialogue, false, null);
                    else
                    {
                        //dialogueBox.GetComponent<Dialogue>().StartDialogue(extraDialogue, false, null);
                        CheckCompletion();
                    }
                }
                else
                {
                    dialogueBox.GetComponent<Dialogue>().StartDialogue(cantGiveMissionDialogue, false, null);
                }
            }
            else
            {
                GiveReward();
            }
        }        
    }

    public override void DisplayNextSentence()
    {
        if (interactable)
        {
            dialogueBox.GetComponent<Dialogue>().DisplayNextSentence();
            if(!missionGiven && hasMission && canGiveMission)
            {
                SpawnNumber();
                missionGiven = true;
                GameEvents.current.CollectableMissionGiven(this.id);
            }                
            if (missionFinished)
            {
                GameEvents.current.CollectableMissionFinished(this.id);
                GameEvents.current.CollectedOrDestroyedObject(this);
                if (hasMission)
                    DestroyNumbers();
                Destroy(gameObject);
            }
        }
    }

    public override void AcceptDialogue()
    {
        if(interactable)
           StartDialogue();
    }

    public override void DeclineDialogue()
    {
        if (interactable)
            StartDialogue();
    }

    private void OnDestroy()
    {
        DestroyNumbers();
    }
}
