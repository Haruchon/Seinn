using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mision_2_3 : Quest
{
    private const string Npc = "Vasija";
    private const string goodDialogue = "Encontraste un interruptor en la vasija! ¿Cuántos faltarán?";
    private const string badDialogue = "Está vacía...";
    private List<DialogueElement> goodPotDialogue;
    private List<DialogueElement> badPotDialogue;
    private bool isEven;

    public GameObject enemyNumberSpawnPoints;

    public GameObject potSpawnPoints;
    public GameObject potObject;

    public GameObject enemySpawnPoint;
    public GameObject enemyObject;

    public override void InitializeParameters()
    {
        goodPotDialogue = new List<DialogueElement>();
        badPotDialogue = new List<DialogueElement>();

        goodPotDialogue.Add(new DialogueElement(Npc, new string[] { goodDialogue }));
        goodPotDialogue.Add(new DialogueElement(Npc, new string[] { goodDialogue }));
        goodPotDialogue.Add(new DialogueElement(Npc, new string[] { goodDialogue }));
        goodPotDialogue.Add(new DialogueElement(Npc, new string[] { goodDialogue }));
        badPotDialogue.Add(new DialogueElement(Npc, new string[] { badDialogue }));
        badPotDialogue.Add(new DialogueElement(Npc, new string[] { badDialogue }));
        badPotDialogue.Add(new DialogueElement(Npc, new string[] { badDialogue }));
        badPotDialogue.Add(new DialogueElement(Npc, new string[] { badDialogue }));
    }

    void Start()
    {
        //InitializeParameters();
        int random = Random.Range(0, 3);
        isEven = (random % 2 == 0) ? true:false;
        id = 10007;
        questOrder = 7;
        questTitle = "Tercer comandante";
        questDescription = "Encuentra todos los interruptores en las vasijas y derrota al enemigo!";
        completed = false;
        acceptDialogue = new DialogueElement("", new string[] { "Creo que primero debo encontrar todos los interruptores de las vasijas" });
        declineDialogue = new DialogueElement("", new string[] { "..." });
        InitializeParameters();

        goals.Add(new CollectionGoal(potObject.GetComponent<CollectableObject>().id, 11, "Encuentra los " + (isEven?"5":"4") + " interruptores escondidos en las vasijas", false, 0, isEven?5:4));
        goals.Add(new KillGoal(enemyObject.GetComponent<Enemy>().id, 12, "Derrota al enemigo y sal de la sala", false, 0, 1));
        goals.ForEach(g => g.Init());

        GameEvents.current.onGoalCompletion += OnFirstGoalCompletion;

    }

    public void OnFirstGoalCompletion(Goal goal)
    {
        if (goal.goalID == 11)
        {
            Destroy(enemySpawnPoint.transform.GetChild(0).gameObject);
            GameObject enemy = Instantiate(enemyObject, enemySpawnPoint.transform.position, Quaternion.identity, enemySpawnPoint.transform) as GameObject;
            enemy.GetComponent<Enemy>().SetSpawner(enemyNumberSpawnPoints, questOrder);
            GameEvents.current.onGoalCompletion -= OnFirstGoalCompletion;
        }
    }

    public override void InitializeObjects()
    {
        base.InitializeObjects();

        int i = 0;
        foreach (Transform child in potSpawnPoints.transform)
        {
            GameObject pot = Instantiate(potObject, child.transform.position, Quaternion.identity, child.transform) as GameObject;
            if((isEven && i % 2 == 0) || (!isEven && i % 2 != 0))
            {
                pot.GetComponent<CollectableObject>().InitializeData(-1, -1, false, goodPotDialogue, questOrder);
                pot.GetComponent<CollectableObject>().EnableInteractions();
            }
            else
            {
                pot.GetComponent<CollectableObject>().InitializeData(-1, -1, false, badPotDialogue, questOrder);
                pot.GetComponent<CollectableObject>().EnableInteractions();
                pot.GetComponent<CollectableObject>().id = 999;
            }
            i++;
        }
    }
}
